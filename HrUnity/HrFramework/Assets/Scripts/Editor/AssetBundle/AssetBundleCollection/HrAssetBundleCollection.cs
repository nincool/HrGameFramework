//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
// 来自GameFramework 学习框架 眼过千遍不如手过一遍
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;

namespace Hr.Editor
{
    /// <summary>
    /// 资源包收集器。
    /// </summary>
    public sealed class HrAssetBundleCollection
    {
        private const string m_c_strAssetBundleNamePattern = @"^([A-Za-z0-9\._-]+/)*[A-Za-z0-9\._-]+$";
        private const string m_c_strAssetBundleVariantPattern = @"^[a-z0-9_-]+$";

        private readonly Dictionary<string, HrAssetBundle> m_dicAssetBundles = new Dictionary<string, HrAssetBundle>();
        private readonly Dictionary<string, HrAsset> m_dicAssets = new Dictionary<string, HrAsset>();

        public int AssetBundleCout
        {
            get
            {
                return m_dicAssetBundles.Count;
            }
        }

        public int AssetCount
        {
            get
            {
                return m_dicAssets.Count;
            }
        }

        public void Clear()
        {
            m_dicAssetBundles.Clear();
            m_dicAssets.Clear();
        }

        public void Load()
        {
            Clear();

        }

        public bool AddAssetBundle(string strAssetBundleName, string strAssetBundleVariant, HrAssetBundleLoadType assetBundleLoadType, bool bAssetBundlePacked)
        {
            if (!IsValidAssetBundleName(strAssetBundleName, strAssetBundleVariant))
            {
                return false;
            }

            if (IsAvailableBundleName(strAssetBundleName, strAssetBundleVariant, null))
            {
                return false;
            }

            HrAssetBundle assetBundle = HrAssetBundle.Create(strAssetBundleName, strAssetBundleVariant, assetBundleLoadType, bAssetBundlePacked);
            m_dicAssetBundles.Add(assetBundle.FullName, assetBundle);

            return true;
        }

        public HrAssetBundle GetAssetBundle(string strAssetBundleName, string strAssetBundleVariant)
        {
            if (!IsValidAssetBundleName(strAssetBundleName, strAssetBundleVariant))
            {
                return null;
            }

            HrAssetBundle assetBundle = m_dicAssetBundles.HrTryGet(GetAssetBundleFullName(strAssetBundleName, strAssetBundleVariant));

            return assetBundle;
        }

        public HrAssetBundle[] GetAssetBundles()
        {
            return m_dicAssetBundles.Values.ToArray();
        }

        /// <summary>
        /// AssetBundle的名字是否合法
        /// </summary>
        /// <param name="strAssetBundleName"></param>
        /// <param name="strAssetBundleVariant"></param>
        /// <returns></returns>
        private bool IsValidAssetBundleName(string strAssetBundleName, string strAssetBundleVariant)
        {
            if (string.IsNullOrEmpty(strAssetBundleName))
            {
                return false;
            }

            if (!Regex.IsMatch(strAssetBundleName, m_c_strAssetBundleNamePattern))
            {
                return false;
            }

            if (strAssetBundleVariant != null && !Regex.IsMatch(strAssetBundleVariant, m_c_strAssetBundleVariantPattern))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 查看名字是否存在
        /// </summary>
        /// <param name="strAssetBundleName"></param>
        /// <param name="strAssetBundleVariant"></param>
        /// <param name="selfAssetBundle">需要校验的AssetBundle</param>
        /// <returns></returns>
        private bool IsAvailableBundleName(string strAssetBundleName, string strAssetBundleVariant, HrAssetBundle selfAssetBundle)
        {
            HrAssetBundle findAssetBundle = GetAssetBundle(strAssetBundleName, strAssetBundleVariant);
            if (findAssetBundle != null)
            {
                return findAssetBundle == selfAssetBundle;
            }

            foreach (HrAssetBundle assetBundle in m_dicAssetBundles.Values)
            {
                if (selfAssetBundle != null && assetBundle == selfAssetBundle)
                {
                    continue;
                }

                if (assetBundle.Name == strAssetBundleName)
                {
                    if (assetBundle.Variant != null && strAssetBundleVariant == assetBundle.Variant)
                    {
                        return true;
                    }

                    if (assetBundle.Variant == null && strAssetBundleVariant == null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// AssetBundle的完整名称，和Unity打出来的AssetBundle一致 name.variant
        /// </summary>
        /// <param name="strAssetBundleName"></param>
        /// <param name="strAssetBundleVariant"></param>
        /// <returns></returns>
        private string GetAssetBundleFullName(string strAssetBundleName, string strAssetBundleVariant)
        {
            return (!string.IsNullOrEmpty(strAssetBundleName) ? string.Format("{0}.{1}", strAssetBundleName, strAssetBundleVariant) : strAssetBundleName);
        }

        /// <summary>
        /// 获取所有的Assets
        /// </summary>
        /// <returns></returns>
        public HrAsset[] GetAssets()
        {
            return m_dicAssets.Values.ToArray();
        }

        /// <summary>
        /// 获取AssetBundle中的Assets
        /// </summary>
        /// <param name="strAssetBundleName"></param>
        /// <param name="strAssetBundleVariant"></param>
        /// <returns></returns>

        public HrAsset[] GetAssets(string strAssetBundleName, string strAssetBundleVariant)
        {
            if (!IsValidAssetBundleName(strAssetBundleName, strAssetBundleVariant))
            {
                return new HrAsset[0];
            }

            HrAssetBundle assetBundle = GetAssetBundle(strAssetBundleName, strAssetBundleVariant);
            if (assetBundle == null)
            {
                return new HrAsset[0];
            }

            return assetBundle.GetAssets();
        }

        /// <summary>
        /// 通过GUID获取对应的Asset
        /// </summary>
        /// <param name="strAssetGUID"></param>
        /// <returns></returns>
        public HrAsset GetAsset(string strAssetGUID)
        {
            if (string.IsNullOrEmpty(strAssetGUID))
            {
                return null;
            }
            HrAsset asset = m_dicAssets.HrTryGet(strAssetGUID);

            return asset;
        }

        public bool HasAsset(string strAssetGUID)
        {
            if (string.IsNullOrEmpty(strAssetGUID))
            {
                return false;
            }

            return m_dicAssets.ContainsKey(strAssetGUID);
        }
    }
}
