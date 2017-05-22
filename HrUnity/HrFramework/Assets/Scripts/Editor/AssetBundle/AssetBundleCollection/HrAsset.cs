//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
// 来自GameFramework 学习框架 眼过千遍不如手过一遍
//------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    /// <summary>
    /// 资源。
    /// </summary>
    public sealed class HrAsset 
    {
        private HrAsset(string guid, HrAssetBundle assetBundle)
        {
            Guid = guid;
            AssetBundle = assetBundle;
        }

        public string Guid
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return AssetDatabase.GUIDToAssetPath(Guid);
            }
        }

        public HrAssetBundle AssetBundle
        {
            get;
            private set;
        }

        public static HrAsset Create(string guid)
        {
            return new HrAsset(guid, null);
        }

        public static HrAsset Create(string guid, HrAssetBundle assetBundle)
        {
            return new HrAsset(guid, assetBundle);
        }

        public void SetAssetBundle(HrAssetBundle assetBundle)
        {
            AssetBundle = assetBundle;
        }
    }

}
