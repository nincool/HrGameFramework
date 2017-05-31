using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor.Hierarchy
{
    public class HrFileItem
    {
        private Texture m_cacheIcon;



        public HrFileItem(string strGUID, HrFileFolder folder)
        {
            if (folder == null)
            {
                Debug.LogWarning("HrFileItem folder is invalid.");
            }
            Guid = strGUID;
            Folder = folder;
        }

        public string Guid
        {
            get;
            private set;
        }

        public string Path
        {
            get
            {
                string strAssetPath = AssetDatabase.GUIDToAssetPath(Guid);
                string[] strFoldersArr = strAssetPath.Split('/');
                Name = strFoldersArr[strFoldersArr.Length - 1];

                return strAssetPath;
            }
        }

        public string Name
        {
            get;
            private set;
        }

        public HrAssetBundle AssetBundle
        {
            get;
            set;
        }

        public HrFileFolder Folder
        {
            get;
            private set;
        }

        public HrFileHierarchyFile FileHierarchyFile
        {
            get;
            set;
        }

        public string FromRootPath
        {
            get
            {
                return (Folder.Folder == null ? Name : string.Format("{0}/{1}", Folder.FromRootPath, Name));
            }
        }

        public int Depth
        {
            get
            {
                return Folder != null ? Folder.Depth + 1 : 0;
            }
        }

        public Texture Icon
        {
            get
            {
                if (m_cacheIcon == null)
                {
                    m_cacheIcon = AssetDatabase.GetCachedIcon(Path);
                }

                return m_cacheIcon;
            }
        }

        /// <summary>
        /// 资源是否被选中 在ContentView中
        /// </summary>
        public bool Selected
        {
            get;
            set;
        }

        public bool IsExisted()
        {
            return HrAssetUtil.IsAssetExisted(Guid);
        }
    }
}
