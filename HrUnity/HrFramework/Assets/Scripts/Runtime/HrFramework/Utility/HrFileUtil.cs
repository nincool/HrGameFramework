using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Hr.Utility
{
    public class HrFileUtil
    {
        /// <summary>
        /// 获取规范的路径。
        /// </summary>
        /// <param name="path">要规范的路径。</param>
        /// <returns>规范的路径。</returns>
        public static string GetRegularPath(string path)
        {
            if (path == null)
            {
                return null;
            }

            return path.Replace('\\', '/');
        }

        /// <summary>
        /// 获取连接后的路径。
        /// </summary>
        /// <param name="path">路径片段。</param>
        /// <returns>连接后的路径。</returns>
        public static string GetCombinePath(params string[] path)
        {
            if (path == null || path.Length < 1)
            {
                return null;
            }

            string combinePath = path[0];
            for (int i = 1; i < path.Length; i++)
            {
                combinePath = System.IO.Path.Combine(combinePath, path[i]);
            }

            return GetRegularPath(combinePath);
        }


        public static string GetPathWithProjectPath(string pathUnderProjectFolder)
        {
            var assetPath = Application.dataPath;
            var projectPath = Directory.GetParent(assetPath).ToString();
            return Path.Combine(projectPath, pathUnderProjectFolder);
        }

        public static string GetPathWithAssetsPath(string pathUnderAssetsFolder)
        {
            var assetPath = Application.dataPath;
            return Path.Combine(assetPath, pathUnderAssetsFolder);
        }

        public static List<string> GetAllFilePathsInFolder(string strLocalFolderPath, string strSuffix = "", bool bIncludeHidden = false, bool bIncludeMeta = false)
        {
            var lisFilePaths = new List<string>();

            if (string.IsNullOrEmpty(strLocalFolderPath))
            {
                return lisFilePaths;
            }
            if (!Directory.Exists(strLocalFolderPath))
            {
                return lisFilePaths;
            }

            GetFilePathsRecursively(strLocalFolderPath, lisFilePaths, strSuffix, bIncludeHidden, bIncludeMeta);

            return lisFilePaths;

        }

        public static List<string> GetFilePathsInFolder(string strFolderPath, string strSuffix = "", bool bIncludeHidden = false, bool bIncludeMeta = false)
        {
            var filePaths = Directory.GetFiles(strFolderPath).Select(p => p);

            if (!bIncludeHidden)
            {
                filePaths = filePaths.Where(path => (new FileInfo(path).Attributes & FileAttributes.Hidden) != FileAttributes.Hidden);
            }

            //if (!bIncludeHidden)
            //{
            //    filePaths = filePaths.Where(path => !(Path.GetFileName(path).StartsWith(".")));
            //}
            if (!bIncludeMeta)
            {
                filePaths = filePaths.Where(path => !HrFileUtil.IsMetaFile(path));
            }
            if (!string.IsNullOrEmpty(strSuffix))
            {
                filePaths = filePaths.Where(path => path.EndsWith(strSuffix));
            }
            // Directory.GetFiles() returns platform dependent delimiter, so make sure replace with "/"
            if (Path.DirectorySeparatorChar != '/')
            {
                filePaths = filePaths.Select(filePath => filePath.Replace(Path.DirectorySeparatorChar.ToString(), "/"));
            }


            return filePaths.ToList();
        }



        public static bool IsMetaFile(string filePath)
        {
            if (filePath.EndsWith(".meta"))
                return true;
            return false;
        }

        public static string GetFileSuffix(string strFileName)
        {
            string[] strArr = strFileName.Split('.');
            if (strArr.Length > 0)
            {
                return strArr[strArr.Length - 1];
            }

            return "";
        }

        public static string GetFileNameWithoutSuffix(string strFileName)
        {
            char[] delimiterChars = { '.', '\\', '/' };
            string[] strArr = strFileName.Split(delimiterChars);
            if (strArr.Length >= 2)
            {
                return strArr[strArr.Length - 2];
            }
            return "";
        }

        public static bool IsDirectory(string path)
        {
            if (path[path.Length - 1] == '/')
            {
                return true;
            }
            return false;
        }

        public static void DelectDir(string strPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(strPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 递归拷贝所有子目录。
        /// </summary>
        /// <param >源目录</param>
        /// <param >目的目录</param>
        public static void CopyDirectory(string sPath, string dPath)
        {
            string[] directories = System.IO.Directory.GetDirectories(sPath);
            if (!System.IO.Directory.Exists(dPath))
                System.IO.Directory.CreateDirectory(dPath);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(sPath);
            System.IO.DirectoryInfo[] dirs = dir.GetDirectories();
            CopyFile(dir, dPath);
            if (dirs.Length > 0)
            {
                foreach (System.IO.DirectoryInfo temDirectoryInfo in dirs)
                {
                    string sourceDirectoryFullName = GetRegularPath(temDirectoryInfo.FullName);
                    string destDirectoryFullName = sourceDirectoryFullName.Replace(sPath, dPath);
                    if (!System.IO.Directory.Exists(destDirectoryFullName))
                    {
                        System.IO.Directory.CreateDirectory(destDirectoryFullName);
                    }
                    CopyFile(temDirectoryInfo, destDirectoryFullName);
                    CopyDirectory(sourceDirectoryFullName, destDirectoryFullName);
                }
            }

        }

        /// <summary>
        /// 拷贝目录下的所有文件到目的目录。
        /// </summary>
        /// <param >源路径</param>
        /// <param >目的路径</param>
        private static void CopyFile(System.IO.DirectoryInfo path, string desPath)
        {
            string sourcePath = GetRegularPath(path.FullName);
            System.IO.FileInfo[] files = path.GetFiles();
            foreach (System.IO.FileInfo file in files)
            {
                string sourceFileFullName = GetRegularPath(file.FullName);
                string destFileFullName = sourceFileFullName.Replace(sourcePath, desPath);
                file.CopyTo(destFileFullName, true);
            }
        }

        private static void GetFilePathsRecursively(string strFolderPath, List<string> lisFilePaths, string strSuffix = "", bool bIncludeHidden = false, bool bIncludeMeta = false)
        {
            var folders = Directory.GetDirectories(strFolderPath);

            foreach (var folder in folders)
            {
                GetFilePathsRecursively(folder, lisFilePaths, strSuffix, bIncludeHidden, bIncludeMeta);
            }

            var files = GetFilePathsInFolder(strFolderPath, strSuffix, bIncludeHidden, bIncludeMeta);
            lisFilePaths.AddRange(files);
        }
    }


}
