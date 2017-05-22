using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public sealed class HrSourceFolder
    {
        private static Texture m_s_cacheIcon = null;

        private readonly List<HrSourceFolder> m_lisFolders;
        private readonly List<HrSourceAsset> m_lisAssets;

        public HrSourceFolder(string strName, HrSourceFolder folder)
        {
            m_lisFolders = new List<HrSourceFolder>();
            m_lisAssets = new List<HrSourceAsset>();

            Name = strName;
            Folder = folder;
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

        public HrSourceFolder[] GetFolders()
        {
            return m_lisFolders.ToArray();
        }

        public HrSourceFolder GetFolder(string strName)
        {
            if (string.IsNullOrEmpty(strName))
            {
                HrLogger.LogError("Source folder name is invalid.");
            }

            foreach (HrSourceFolder folder in m_lisFolders)
            {
                if (folder.Name == strName)
                {
                    return folder;
                }
            }

            return null;
        }

        public HrSourceFolder AddFolder(string strName)
        {
            if (string.IsNullOrEmpty(strName))
            {
                HrLogger.LogError("Srouce folder name is invalid.");
            }

            HrSourceFolder folder = GetFolder(strName);
            if (folder != null)
            {
                HrLogger.LogError("Source folder is already exist.");
            }

            folder = new HrSourceFolder(strName, this);
            m_lisFolders.Add(folder);

            return folder;
        }

        public HrSourceAsset[] GetAssets()
        {
            return m_lisAssets.ToArray();
        }

        public HrSourceAsset GetAsset(string strName)
        {
            if (string.IsNullOrEmpty(strName))
            {
                HrLogger.LogError("Source asset name is invalid.");
            }
            foreach (HrSourceAsset asset in m_lisAssets)
            {
                if (asset.Name == strName)
                {
                    return asset;
                }
            }

            return null;
        }

        public HrSourceAsset AddAsset(string strGUID, string strPath, string strName)
        {
            if (string.IsNullOrEmpty(strGUID))
            {
                HrLogger.LogError("Source asset guid is invalid.");
            }

            if (string.IsNullOrEmpty(strPath))
            {
                HrLogger.LogError("Source asset path is invalid.");
            }

            if (string.IsNullOrEmpty(strName))
            {
                HrLogger.LogError("Source asset name is invalid.");
            }

            HrSourceAsset asset = GetAsset(strName);
            if (asset != null)
            {
                HrLogger.LogError(string.Format("Source asset '{0}' is already exist.", strName));
            }

            asset = new HrSourceAsset(strGUID, strPath, strName, this);
            m_lisAssets.Add(asset);

            return asset;
        }
    }
}
