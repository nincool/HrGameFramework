using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetBundleItem
    {
        private static Texture s_CachedUnknownIcon = null;
        private static Texture s_CachedAssetIcon = null;
        private static Texture s_CachedSceneIcon = null;

        public HrAssetBundleItem(string name, HrAssetBundle assetBundle, HrAssetBundleFolder folder)
        {
            if (assetBundle == null)
            {
                HrLogger.LogError("AssetBundle is invalid.");
            }

            if (folder == null)
            {
                HrLogger.LogError("AssetBundle folder is invalid.");
            }

            Name = name;
            AssetBundle = assetBundle;
            Folder = folder;
        }

        public string Name
        {
            get;
            private set;
        }

        public HrAssetBundle AssetBundle
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
                switch (AssetBundle.Type)
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

    }
}

