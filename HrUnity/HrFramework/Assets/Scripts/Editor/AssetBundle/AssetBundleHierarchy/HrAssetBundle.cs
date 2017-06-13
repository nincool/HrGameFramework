//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
// 来自GameFramework 学习框架 眼过千遍不如手过一遍
//------------------------------------------------------------
using Hr.Editor.Hierarchy;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    /// <summary>
    /// 资源包。
    /// </summary>
    public sealed class HrAssetBundle 
    {
        private static Texture s_CachedUnknownIcon = null;
        private static Texture s_CachedAssetIcon = null;
        private static Texture s_CachedSceneIcon = null;

        private readonly List<HrFileItem> m_Assets;

        private HrAssetBundle(string name, string variant)
        {
            m_Assets = new List<HrFileItem>();

            Name = name;
            Variant = variant;
            Type = HrAssetBundleType.Unknown;
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
                return !string.IsNullOrEmpty(Variant) ? string.Format("{0}.{1}", Name, Variant) : Name;
            }
        }

        private static Texture CachedUnknownIcon
        {
            get
            {
                if (s_CachedUnknownIcon == null)
                {
                    s_CachedUnknownIcon = EditorGUIUtility.IconContent("Prefab Icon").image;
                }

                return s_CachedUnknownIcon;
            }
        }

        private static Texture CachedAssetIcon
        {
            get
            {
                if (s_CachedAssetIcon == null)
                {
                    s_CachedAssetIcon = EditorGUIUtility.IconContent("PrefabNormal Icon").image;
                }

                return s_CachedAssetIcon;
            }
        }

        private static Texture CachedSceneIcon
        {
            get
            {
                if (s_CachedSceneIcon == null)
                {
                    s_CachedSceneIcon = EditorGUIUtility.IconContent("SceneAsset Icon").image;
                }

                return s_CachedSceneIcon;
            }
        }



        public Texture Icon
        {
            get
            {
                switch (Type)
                {
                    case HrAssetBundleType.Asset:
                        return CachedAssetIcon;
                    case HrAssetBundleType.Scene:
                        return CachedSceneIcon;
                    default:
                        return CachedUnknownIcon;
                }
            }
        }

        public HrAssetBundleType Type
        {
            get;
            private set;
        }

        public bool Selected
        {
            get;
            set;
        }

        public static HrAssetBundle Create(string name, string variant)
        {
            return new HrAssetBundle(name, variant);
        }

        public HrFileItem[] GetAssets()
        {
            return m_Assets.ToArray();
        }

        public void Rename(string name, string variant)
        {
            Name = name;
            Variant = variant;
        }

         public void AssignAsset(HrFileItem asset, bool isScene)
        {
            if (asset.AssetBundle != null)
            {
                asset.AssetBundle.Unassign(asset);
            }

            Type = isScene ? HrAssetBundleType.Scene : HrAssetBundleType.Asset;
            asset.AssetBundle = this;
            m_Assets.Add(asset);
            m_Assets.Sort(AssetComparer);
        }

        public void Unassign(HrFileItem asset)
        {
            asset.AssetBundle = null;
            m_Assets.Remove(asset);
            if (m_Assets.Count <= 0)
            {
                Type = HrAssetBundleType.Unknown;
            }
        }

        public void Clear()
        {
            foreach (HrFileItem asset in m_Assets)
            {
                asset.AssetBundle = null;
            }

            m_Assets.Clear();
        }

        private int AssetComparer(HrFileItem a, HrFileItem b)
        {
            return a.Guid.CompareTo(b.Guid);
        }
    }
}
