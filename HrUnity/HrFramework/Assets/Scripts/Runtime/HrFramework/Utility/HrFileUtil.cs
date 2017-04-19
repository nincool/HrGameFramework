using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Hr
{
    public class HrFileUtil
    {
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

        public static List<string> GetAllFilePathsInFolder(string strLocalFolderPath, bool bIncludeHidden = false, bool bIncludeMeta = false)
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

            GetFilePathsRecursively(strLocalFolderPath, lisFilePaths, bIncludeHidden, bIncludeMeta);

            return lisFilePaths;

        }

        public static List<string> GetFilePathsInFolder(string strFolderPath, bool bIncludeHidden = false, bool bIncludeMeta = false)
        {
            var filePaths = Directory.GetFiles(strFolderPath).Select(p => p);

            if (!bIncludeHidden)
            {
                filePaths = filePaths.Where(path => !(Path.GetFileName(path).StartsWith(".")));
            }
            if (!bIncludeMeta)
            {
                filePaths = filePaths.Where(path => !HrFileUtil.IsMetaFile(path));
            }

            // Directory.GetFiles() returns platform dependent delimiter, so make sure replace with "/"
            if (Path.DirectorySeparatorChar != '/')
            {
                filePaths = filePaths.Select(filePath => filePath.Replace(Path.DirectorySeparatorChar.ToString(), "/"));
            }

            return filePaths.ToList();
        }

        private static void GetFilePathsRecursively(string strFolderPath, List<string> lisFilePaths, bool bIncludeHidden = false, bool bIncludeMeta = false)
        {
            var folders = Directory.GetDirectories(strFolderPath);

            foreach (var folder in folders)
            {
                GetFilePathsRecursively(folder, lisFilePaths, bIncludeHidden, bIncludeMeta);
            }

            var files = GetFilePathsInFolder(strFolderPath, bIncludeHidden, bIncludeMeta);
            lisFilePaths.AddRange(files);
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
    }


}
