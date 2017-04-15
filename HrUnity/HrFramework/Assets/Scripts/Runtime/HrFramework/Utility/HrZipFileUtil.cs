using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Hr.CommonUtility
{
    public class HrUnpackZipFileThread
    {
        private Action<long, long> m_actProgress = null;

        private byte[] m_memoryDatas = null;

        private string m_strDesPath = null;

        private bool m_bFinished = false;
        public bool IsDone
        {
            get { return m_bFinished; }
        }

        public HrUnpackZipFileThread(byte[] datas, string strDesPath, Action<long, long> actProgress)
        {
            m_memoryDatas = datas;
            m_strDesPath = strDesPath;
            m_actProgress = actProgress;
        }

        public void Start()
        {
            HrLoger.Log("UnpackZipFile Start! DesPath:" + m_strDesPath);
            if (!Directory.Exists(m_strDesPath))
            {
                Directory.CreateDirectory(m_strDesPath);
            }
            Thread thread = new Thread(UnpackFiles);
            thread.Start();
        }

        private void UnpackFiles()
        {
            ZipEntry zip = null;

            Stream stream = new MemoryStream(m_memoryDatas);

            long nStreamLength = stream.Length;
            long nReadSize = 0;
            ZipInputStream zipInStream = new ZipInputStream(stream);
            while ((zip = zipInStream.GetNextEntry()) != null)
            {
                UnzipFile(zip, zipInStream, m_strDesPath);
                nReadSize = zipInStream.Position;

                if (m_actProgress != null)
                {
                    m_actProgress(nReadSize, nStreamLength);
                }
            }

            try
            {
                zipInStream.Close();
            }
            catch (Exception ex)
            {
                HrLoger.LogError(ex.ToString());
                throw ex;
            }

            if (m_actProgress != null)
            {
                m_actProgress(1, 1);
            }

            m_bFinished = true;
        }

        private void UnzipFile(ZipEntry zip, ZipInputStream zipInStream, string dirPath)
        {
            try
            {
                //文件名不为空    
                if (!string.IsNullOrEmpty(zip.Name))
                {
                    string directoryName = Path.GetDirectoryName(zip.Name);
                    string fileName = Path.GetFileName(zip.Name);

                    string strFilePath = dirPath;
                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        strFilePath += directoryName;
                    }
                    strFilePath.Replace("\\", "/");
                    if (!Directory.Exists(strFilePath))
                    {
                        Directory.CreateDirectory(strFilePath);
                    }
                    string strFullFileName = strFilePath + "/" + fileName;

                    //如果是一个新的文件路径　这里需要创建这个文件路径    
                    FileStream fs = null;
                    //当前文件夹下有该文件  删掉  重新创建    
                    if (File.Exists(strFullFileName))
                    {
                        File.Delete(strFullFileName);
                    }

                    fs = File.Create(strFullFileName);
                    int size = 0;
                    byte[] data = new byte[2048];
                    //每次读取2MB  直到把这个内容读完    
                    while (true)
                    {
                        size = zipInStream.Read(data, 0, data.Length);
                        //小于0， 也就读完了当前的流    
                        if (size > 0)
                        {
                            fs.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }

                    fs.Close();
                }
            }
            catch (Exception e)
            {
                HrLoger.LogError(e.ToString());
            }
        }

    }

    public class HrZipFileUtil
    {

        /// <summary>压缩文件</summary>  
        /// <param name="filename">filename生成的文件的名称，如：C\123\123.zip</param>  
        /// <param name="directory">directory要压缩的文件夹路径</param>  
        /// <returns></returns>  
        public static bool PackFiles(string filename, string directory)
        {
            try
            {
                directory = directory.Replace("/", "\\");

                if (!directory.EndsWith("\\"))
                    directory += "\\";

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                FastZip fz = new FastZip();
                fz.CreateEmptyDirectories = true;
                fz.CreateZip(filename, directory, true, "");

                return true;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return false;
            }
        }


        /// <summary>解压文件</summary>  
        /// <param name="file">压缩文件的名称，如：C:\123\123.zip</param>  
        /// <param name="dir">dir要解压的文件夹路径</param>  
        /// <returns></returns>  
        public static bool UnpackFiles(string file, string dir)
        {
            try
            {
                if (!File.Exists(file))
                    return false;
                dir = dir.Replace("/", "\\");
                if (!dir.EndsWith("\\"))
                    dir += "\\";
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                ZipInputStream s = new ZipInputStream(File.OpenRead(file));
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)

                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    if (directoryName != String.Empty)
                        Directory.CreateDirectory(dir + directoryName);
                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(dir + theEntry.Name);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        streamWriter.Close();
                    }
                }
                s.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
