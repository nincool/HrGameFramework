using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public sealed class HrAssetBundleFolder
    {
        private static Texture m_cacheIcon = null;

        private readonly List<HrAssetBundleFolder> m_lisFolders;
        private readonly List<HrAssetBundleItem> m_lisItems;

        public HrAssetBundleFolder(string name, HrAssetBundleFolder folder)
        {
            m_lisFolders = new List<HrAssetBundleFolder>();
            m_lisItems = new List<HrAssetBundleItem>();

            Name = name;
            Folder = folder;
        }

        public string Name
        {
            get;
            private set;
        }

        public HrAssetBundleFolder Folder
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
                if (m_cacheIcon == null)
                {
                    m_cacheIcon = AssetDatabase.GetCachedIcon("Assets");
                }

                return m_cacheIcon;
            }
        }

        public void Clear()
        {
            m_lisFolders.Clear();
            m_lisItems.Clear();
        }

        public HrAssetBundleFolder[] GetFolders()
        {
            return m_lisFolders.ToArray();
        }

        public HrAssetBundleFolder GetFolder(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                HrLogger.LogError("AssetBundle folder name is invalid.");
            }

            foreach (HrAssetBundleFolder folder in m_lisFolders)
            {
                if (folder.Name == name)
                {
                    return folder;
                }
            }

            return null;
        }

        public HrAssetBundleFolder AddFolder(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                HrLogger.LogError("AssetBundle folder name is invalid.");
            }

            HrAssetBundleFolder folder = GetFolder(name);
            if (folder != null)
            {
                HrLogger.LogError("AssetBundle folder is already exist.");
            }

            folder = new HrAssetBundleFolder(name, this);
            m_lisFolders.Add(folder);

            return folder;
        }

        public HrAssetBundleItem[] GetItems()
        {
            return m_lisItems.ToArray();
        }

        public HrAssetBundleItem GetItem(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                HrLogger.LogError("AssetBundle item name is invalid.");
            }

            foreach (HrAssetBundleItem item in m_lisItems)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }

            return null;
        }

        public void AddItem(string name, HrAssetBundle assetBundle)
        {
            HrAssetBundleItem item = GetItem(name);
            if (item != null)
            {
                HrLogger.LogError("AssetBundle item is already exist.");
            }

            item = new HrAssetBundleItem(name, assetBundle, this);
            m_lisItems.Add(item);
            m_lisItems.Sort(AssetBundleItemComparer);
        }

        private int AssetBundleItemComparer(HrAssetBundleItem a, HrAssetBundleItem b)
        {
            return a.Name.CompareTo(b.Name);
        }
    }
}
