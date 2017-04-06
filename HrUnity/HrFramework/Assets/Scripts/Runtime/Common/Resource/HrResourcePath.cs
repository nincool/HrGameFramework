using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr.CommonUtility;

namespace Hr.Resource
{
    public class HrResourcePath
    {
        public static readonly string mStrProjectplatform;

        public static readonly string mStrStreamingAssetBundlePath;
        //数据持久化目录
        public static readonly string mStrPersistentAssetBundlePath;

        static HrResourcePath()
        {
            string strPlatformName = HrAssetBundleUtility.GetPlatformName();

            //if (Application.isEditor)
            //{
            //    mStrStreamingAssetBundlePath = "file://" + Application.dataPath + "/../../.." + AssetBundlePathInStream + "/" + ProjectPlatform;
            //}
            //mStrStreamingAssetBundlePath = mStrSteamAssetsPath + "/" + strPlatformName + "/";
        }


    }

}

