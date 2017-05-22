//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
// 来自GameFramework 学习框架 眼过千遍不如手过一遍
//------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Editor
{
    /// <summary>
    /// 资源包。
    /// </summary>
    public sealed class HrAssetBundle 
    {
        private readonly List<HrAsset> m_Assets;

        private HrAssetBundle(string name, string variant, HrAssetBundleLoadType loadType, bool packed)
        {
            m_Assets = new List<HrAsset>();

            Name = name;
            Variant = variant;
            Type = HrAssetBundleType.Unknown;
            LoadType = loadType;
            Packed = packed;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Variant
        {
            get;
            private set;
        }

        public string FullName
        {
            get
            {
                return Variant != null ? string.Format("{0}.{1}", Name, Variant) : Name;
            }
        }

        public HrAssetBundleType Type
        {
            get;
            private set;
        }

        public HrAssetBundleLoadType LoadType
        {
            get;
            private set;
        }

        public bool Packed
        {
            get;
            private set;
        }

        public static HrAssetBundle Create(string name, string variant, HrAssetBundleLoadType loadType, bool packed)
        {
            return new HrAssetBundle(name, variant, loadType, packed);
        }

        public HrAsset[] GetAssets()
        {
            return m_Assets.ToArray();
        }

        public void Rename(string name, string variant)
        {
            Name = name;
            Variant = variant;
        }

        public void SetLoadType(HrAssetBundleLoadType loadType)
        {
            LoadType = loadType;
        }

        public void SetPacked(bool packed)
        {
            Packed = packed;
        }

        public void AssignAsset(HrAsset asset, bool isScene)
        {
            if (asset.AssetBundle != null)
            {
                asset.AssetBundle.Unassign(asset);
            }

            Type = isScene ? HrAssetBundleType.Scene : HrAssetBundleType.Asset;
            asset.SetAssetBundle(this);
            m_Assets.Add(asset);
            m_Assets.Sort(AssetComparer);
        }

        public void Unassign(HrAsset asset)
        {
            asset.SetAssetBundle(null);
            m_Assets.Remove(asset);
            if (m_Assets.Count <= 0)
            {
                Type = HrAssetBundleType.Unknown;
            }
        }

        public void Clear()
        {
            foreach (HrAsset asset in m_Assets)
            {
                asset.SetAssetBundle(null);
            }

            m_Assets.Clear();
        }

        private int AssetComparer(HrAsset a, HrAsset b)
        {
            return a.Guid.CompareTo(b.Guid);
        }
    }
}
