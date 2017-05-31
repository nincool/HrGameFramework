using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Editor.Hierarchy
{
    public class HrFileHierarchyFolder
    {

        private HrFileFolder m_sourceFolder;

        private bool m_bSelected = false;
        /// <summary>
        /// 深度
        /// </summary>
        public int Depth
        {
            get
            {
                return m_sourceFolder.Depth;
            }
        }

        /// <summary>
        /// 在层级的底基层
        /// </summary>
        public int Row
        {
            get;
            set;
        }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool Expanded
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                return m_sourceFolder.Name;
            }
        }

        /// <summary>
        /// 持有的文件夹
        /// </summary>
        public HrFileFolder SourceFolder
        {
            get
            {
                return m_sourceFolder;
            }
        }

        public static Texture Icon
        {
            get
            {
                return HrFileFolder.Icon;
            }
        }



        public HrFileHierarchyFolder(HrFileFolder folder)
        {
            m_sourceFolder = folder;
            folder.FileHierarchyFolder = this;
            Expanded = true;
            
        }

        public HrFileHierarchyFolder GetFileHierarchyFolder(string strFolderName)
        {
            HrFileFolder folder = SourceFolder.GetFolder(strFolderName);
            if (folder != null)
            {
                return folder.FileHierarchyFolder;
            }

            return null;
        }

        /// <summary>
        /// 这里存在效率问题，TODO缓存
        /// </summary>
        /// <returns></returns>
        public List<HrFileHierarchyFolder> GetFileHierarchyFolders()
        {
            List<HrFileHierarchyFolder> lisFolder = new List<HrFileHierarchyFolder>();
            var folders = SourceFolder.GetFolders();
            foreach (var folder in folders)
            {
                lisFolder.Add(folder.FileHierarchyFolder);
            }
            return lisFolder;
        }

        public HrFileHierarchyFolder AddFileHierarchyFolder(string strFolderName)
        {
            HrFileFolder folder = SourceFolder.GetFolder(strFolderName);
            if (folder != null)
            {
                return folder.FileHierarchyFolder;
            }
            else
            {
                HrFileFolder newFolder = SourceFolder.AddFolder(strFolderName);
                HrFileHierarchyFolder fileHierarchyFolder = new HrFileHierarchyFolder(newFolder);

                return fileHierarchyFolder;
            }
        }

        public HrFileHierarchyFile GetFileHierarchyFile(string strGUID)
        {
            HrFileItem file = SourceFolder.GetAsset(strGUID);
            if (file != null)
            {
                return file.FileHierarchyFile;
            }
            else
            {
                return null;
            }
        }

        public List<HrFileHierarchyFile> GetFileHierarchyFiles()
        {
            List<HrFileHierarchyFile> lisHierarchyFile = new List<HrFileHierarchyFile>();
            var subFiles = SourceFolder.GetAssets();
            foreach (var file in subFiles)
            {
                lisHierarchyFile.Add(file.FileHierarchyFile);
            }

            return lisHierarchyFile;
        }

        public HrFileHierarchyFile AddFileHierarchyFile( string strGUID)
        {
            HrFileItem file = SourceFolder.GetAsset(strGUID);
            if (file != null)
            {
                return file.FileHierarchyFile;
            }
            else
            {
                HrFileItem newFile = SourceFolder.AddAsset(strGUID);
                HrFileHierarchyFile fileHierarchyFile = new HrFileHierarchyFile(newFile);

                return fileHierarchyFile;
            }
        }

        public bool IsSelected()
        {
            return m_bSelected;
        }

        public void SetSelected(bool bSelected)
        {
            m_bSelected = bSelected;

            var lisHierarchyFolders = GetFileHierarchyFolders();
            foreach (var subFolder in lisHierarchyFolders)
            {
                subFolder.SetSelected(m_bSelected);
            }
            var lisHierarchyFiles = GetFileHierarchyFiles();
            foreach (var subFile in lisHierarchyFiles)
            {
                subFile.Selected = m_bSelected;
            }
        }
    }
}

















