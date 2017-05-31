using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Hr.Editor.Hierarchy;

namespace Hr.Editor
{
    public class HrAssetBundleFolder
    {
        private static Texture m_s_cacheIcon = null;

        private readonly Dictionary<string, HrAssetBundle> m_dicAssetBundles = new Dictionary<string, HrAssetBundle>();

        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool Expanded
        {
            get;
            set;
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

        public Dictionary<string, HrAssetBundle> AssetBundles
        {
            get
            {
                return m_dicAssetBundles;
            }
        }

        public HrAssetBundleFolder(string strFolderName)
        {
            Name = strFolderName;
        }

        public HrAssetBundle GetAssetBundle(string strAssetBundleFullName)
        {
            return m_dicAssetBundles.HrTryGet(strAssetBundleFullName);
        }

        public HrAssetBundle[] GetAssetBundles()
        {
            return m_dicAssetBundles.Values.ToArray();
        }

        public List<HrFileItem> GetAssets()
        {
            List<HrFileItem> lisFileItem = new List<HrFileItem>();
            foreach (var assetBundle in m_dicAssetBundles.Values)
            {
                lisFileItem.AddRange(assetBundle.GetAssets());
            }

            return lisFileItem;
        }

        public bool RemoveAssetBundle(string strAssetBundleFullName)
        {
            var assetBundle = GetAssetBundle(strAssetBundleFullName);
            if (assetBundle != null)
            {
                var assetsArr = assetBundle.GetAssets();
                foreach (var asset in assetsArr)
                {
                    assetBundle.Unassign(asset);
                }
                m_dicAssetBundles.Remove(strAssetBundleFullName);
                return true;
            }

            return false;
        }
    }
}
