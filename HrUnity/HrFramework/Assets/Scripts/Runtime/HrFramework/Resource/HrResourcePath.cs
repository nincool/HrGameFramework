using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr.CommonUtility;

namespace Hr.Resource
{
    public class HrResourcePath
    {
        public const string STR_ASSETBUNDLES_OUTPUT_PATH = "../../AssetBundles/";
        public const string STR_ZIP_ASSETFILE = "assets.zip";

        public static readonly string sStrProjectplatform;

        //AssetBundle打包输出目录
        public static readonly string sAssetBundleOutputPath;

        //AssetBundle压缩为zip存放目录 
        public static readonly string sZipAssetBundlePath;

        //AssetBundle解压缩目录
        public static readonly string sZipAssetBundleUnPackPath;

        //StreamingAsset www加载路径
        public static readonly string sStreamingAssetPathForWWW;


        public static readonly string mStrStreamingAssetBundlePath;

        //数据持久化目录
        public static readonly string mStrPersistentAssetBundlePath;

        static HrResourcePath()
        {
            sStrProjectplatform = HrAssetBundleUtility.GetPlatformName();
            sAssetBundleOutputPath = Application.dataPath + "/" + STR_ASSETBUNDLES_OUTPUT_PATH;
            sZipAssetBundlePath = Application.streamingAssetsPath + "/" + STR_ZIP_ASSETFILE;
            sZipAssetBundleUnPackPath = Application.persistentDataPath + "/" + "AssetBundles/";


            if (sStrProjectplatform == "Android")
            {
                sStreamingAssetPathForWWW = Application.streamingAssetsPath + "/";
            }
            else
            {
                sStreamingAssetPathForWWW = "file://" + Application.streamingAssetsPath + "/";
            }
            mStrPersistentAssetBundlePath = Application.persistentDataPath + "/" + "AssetBundles/" + sStrProjectplatform + "/";
        }
        
        public static string CombineAssetBundlePath(string assetPath)
        {
            return mStrPersistentAssetBundlePath + assetPath;
        }

        public static string GetAssetBundlePath()
        {
            return mStrPersistentAssetBundlePath;
        }

        public static string GetStreamingAssetBundlePath()
        {
            return mStrStreamingAssetBundlePath;
        }

    }

}



