using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr.CommonUtility;

namespace Hr.Resource
{
    public class HrResourcePath
    {
        public const string STR_ASSETBUNDLES_OUTPUT_PATH = "../../AssetBundles";

        public static readonly string sStrProjectplatform;

        //AssetBundle打包输出目录
        public static readonly string sAssetBundleOutputPath;

        //AssetBundle压缩为zip存放目录 
        public static readonly string sZipAssetBundlePath;

        public static readonly string mStrStreamingAssetBundlePath;
        //数据持久化目录
        public static readonly string mStrPersistentAssetBundlePath;

        static HrResourcePath()
        {
            sStrProjectplatform = HrAssetBundleUtility.GetPlatformName();
            sAssetBundleOutputPath = Application.dataPath + "/" + STR_ASSETBUNDLES_OUTPUT_PATH;
            sZipAssetBundlePath = Application.streamingAssetsPath + "/assets.zip";
        }

        

    }

}



