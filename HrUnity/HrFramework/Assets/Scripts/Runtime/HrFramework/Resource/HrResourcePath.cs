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
        public static readonly string ms_strAssetBundleOutputPath;

        //AssetBundle压缩为zip存放目录 
        public static readonly string ms_strZipAssetBundlePath;

        //AssetBundle解压缩目录
        public static readonly string ms_strZipAssetBundleUnPackPath;

        //StreamingAsset www加载路径
        public static readonly string ms_strStreamingAssetPathForWWW;


        public static readonly string ms_strStreamingAssetBundlePath;

        //数据持久化目录
        public static readonly string ms_strPersistentAssetBundlePath;

        //PC端项目目录
        public static readonly string ms_strWorkingPath = Application.dataPath + "/../";

        static HrResourcePath()
        {
            sStrProjectplatform = HrAssetBundleUtility.GetPlatformName();
            ms_strAssetBundleOutputPath = Application.dataPath + "/" + STR_ASSETBUNDLES_OUTPUT_PATH;
            ms_strZipAssetBundlePath = Application.streamingAssetsPath + "/" + STR_ZIP_ASSETFILE;
            ms_strZipAssetBundleUnPackPath = Application.persistentDataPath + "/" + "AssetBundles/";


            if (sStrProjectplatform == "Android")
            {
                ms_strStreamingAssetPathForWWW = Application.streamingAssetsPath + "/";
            }
            else
            {
                ms_strStreamingAssetPathForWWW = "file://" + Application.streamingAssetsPath + "/";
            }
            ms_strPersistentAssetBundlePath = Application.persistentDataPath + "/" + "AssetBundles/" + sStrProjectplatform + "/";
        }
        
        public static string CombineAssetBundlePath(string assetPath)
        {
            return ms_strPersistentAssetBundlePath + assetPath;
        }

        public static string GetAssetBundlePath()
        {
            return ms_strPersistentAssetBundlePath;
        }

        public static string GetStreamingAssetBundlePath()
        {
            return ms_strStreamingAssetBundlePath;
        }

    }

}



