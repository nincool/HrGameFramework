using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetBundleEditorController
    {
        //默认路径 最好不要设置为Assets，不然资源太多
        private const string m_strDefaultSourceAssetRootPath = "Assets/Media";

        private readonly HrAssetBundleCollection m_assetBundleCollection = new HrAssetBundleCollection();

        private readonly List<string> m_lisSourceAssetSearchPaths = new List<string>();
        //相对根路径的子目录
        private readonly List<string> m_lisSourceAssetSearchRelativePaths = new List<string>();

        private HrSourceFolder m_sourceAssetRoot;
        //搜索路径的根路径
        private string m_strSourceAssetRootPath;

        public HrAssetBundleEditorController()
        {
            RefreshSourceAssetSearchPaths();
        }

        public int AssetBundleCount
        {
            get
            {
                return m_assetBundleCollection.AssetBundleCout;
            }
        }

        public int AssetCount
        {
            get
            {
                return m_assetBundleCollection.AssetCount;
            }
        }

        public HrSourceFolder SourceAssetRoot
        {
            get
            {
                return m_sourceAssetRoot;
            }
        }

        public string SourceAssetRootPath
        {
            get
            {
                return m_strSourceAssetRootPath;
            }
            set
            {
                if (m_strSourceAssetRootPath == value)
                {
                    return;
                }
                m_strSourceAssetRootPath = value.Replace('\\', '/');
                m_sourceAssetRoot = new HrSourceFolder(m_strSourceAssetRootPath, null);
            }
        }


        private void RefreshSourceAssetSearchPaths()
        {
            m_lisSourceAssetSearchPaths.Clear();

            if (string.IsNullOrEmpty(m_strSourceAssetRootPath))
            {
                SourceAssetRootPath = m_strDefaultSourceAssetRootPath;
            }

            m_lisSourceAssetSearchPaths.Add(m_strSourceAssetRootPath);
            //todo 是否需要查找下子目录
        }

        public bool AddAssetBundle(string strAssetBundleName, string strAssetBundleVariant, HrAssetBundleLoadType assetBundleLoadType, bool bAssetBundlePacked)
        {
            return m_assetBundleCollection.AddAssetBundle(strAssetBundleName, strAssetBundleVariant, assetBundleLoadType, bAssetBundlePacked);
        }

        public HrAssetBundle[] GetAssetBundles()
        {
            return m_assetBundleCollection.GetAssetBundles();
        }

        public HrAsset GetAsset(string strAssetGUID)
        {
            return m_assetBundleCollection.GetAsset(strAssetGUID);
        }
    }
}

