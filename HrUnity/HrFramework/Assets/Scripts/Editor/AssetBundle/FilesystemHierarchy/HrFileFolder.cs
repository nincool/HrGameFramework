using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor.Hierarchy
{
    public class HrFileFolder
    {
        private static Texture m_s_cacheIcon = null;

        private readonly List<HrFileFolder> m_lisFolders;
        private readonly List<HrFileItem> m_lisAssets;

        public HrFileFolder(string strName, HrFileFolder folder)
        {
            m_lisFolders = new List<HrFileFolder>();
            m_lisAssets = new List<HrFileItem>();

            Name = strName;
            Folder = folder;
        }

        public string Name
        {
            get;
            private set;
        }

        public HrFileFolder Folder
        {
            get;
            private set;
        }

        public HrFileHierarchyFolder FileHierarchyFolder
        {
            get;
            set;
        }

        public string FromRootPath
        {
            get
            {
                return Folder == null ? string.Empty : (Folder.Folder == null ? Name : string.Format("{0}/{1}", Folder.FromRootPath, Name));
            }
        }

        public int Depth
        {
            get
            {
                return Folder != null ? Folder.Depth + 1 : 0;
            }
        }

        public static Texture Icon
        {
            get
            {
                if (m_s_cacheIcon == null)
                {
                    m_s_cacheIcon = AssetDatabase.GetCachedIcon("Assets");
                }

                return m_s_cacheIcon;
            }
        }


        public void Clear()
        {
            m_lisFolders.Clear();
            m_lisAssets.Clear();
        }

        public HrFileFolder[] GetFolders()
        {
            return m_lisFolders.ToArray();
        }

        public HrFileFolder GetFolder(string strName)
        {
            if (string.IsNullOrEmpty(strName))
            {
                Debug.LogError("Source folder name is invalid.");
            }

            foreach (HrFileFolder folder in m_lisFolders)
            {
                if (folder.Name == strName)
                {
                    return folder;
                }
            }

            return null;
        }

        public HrFileFolder AddFolder(string strName)
        {
            if (string.IsNullOrEmpty(strName))
            {
                Debug.LogError("Source folder name is invalid.");
            }

            HrFileFolder folder = GetFolder(strName);
            if (folder != null)
            {
                Debug.LogError("Source folder is already exist.");
            }

            folder = new HrFileFolder(strName, this);
            m_lisFolders.Add(folder);

            return folder;
        }

        public HrFileItem[] GetAssets()
        {
            return m_lisAssets.ToArray();
        }

        public HrFileItem GetAsset(string strGUID)
        {
            if (string.IsNullOrEmpty(strGUID))
            {
                Debug.LogError("Source asset guid is invalid.");
            }
            foreach (HrFileItem asset in m_lisAssets)
            {
                if (asset.Guid == strGUID)
                {
                    return asset;
                }
            }

            return null;
        }

        public HrFileItem AddAsset(string strGUID)
        {
            if (string.IsNullOrEmpty(strGUID))
            {
                Debug.LogError(string.Format("Source asset '{0}' is not existed.", strGUID));
            }

            if (!HrAssetUtil.IsAssetExisted(strGUID))
            {
                Debug.LogError(string.Format("Source asset '{0}' is not existed.", strGUID));
            }

            HrFileItem asset = GetAsset(strGUID);
            if (asset != null)
            {
                Debug.LogError(string.Format("Source asset '{0}' is already exist.", asset.Path));
            }

            asset = new HrFileItem(strGUID, this);
            m_lisAssets.Add(asset);

            return asset;
        }

    }
}
