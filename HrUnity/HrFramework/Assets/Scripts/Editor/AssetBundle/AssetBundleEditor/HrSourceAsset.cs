using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hr.Editor
{
    public sealed class HrSourceAsset
    {
        private Texture m_cacheIcon;
        
        public HrSourceAsset(string strGUID, string strPath, string strName, HrSourceFolder folder)
        {
            if (folder == null)
            {
                HrLogger.LogError("Source asset folder is invalid.");
            }

        }

        public string Guid
        {
            get;
            private set;
        }

        public string Path
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public HrSourceFolder Folder
        {
            get;
            private set;
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
    }
}
