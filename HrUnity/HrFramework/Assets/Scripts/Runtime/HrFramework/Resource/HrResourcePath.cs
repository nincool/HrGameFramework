using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr;

namespace Hr
{
    public class HrResourcePath
    {
        public const string STR_ASSETBUNDLES_OUTPUT_PATH = "../../AssetBundles/";
        public const string STR_ZIP_ASSETFILE = "assets.zip";

        public static readonly string ms_strProjectplatform;

        //AssetBundle打包输出目录
        public static readonly string ms_strAssetBundleOutputPath;

        //AssetBundle压缩为zip存放目录 
        public static readonly string ms_strZipAssetBundlePath;

        //AssetBundle解压缩目录
        public static readonly string ms_strZipAssetBundleUnPackPath;

        //StreamingAsset www加载路径
        public static readonly string ms_strStreamingAssetPathForWWW;

        //AssetBundle Streaming目录
        public static readonly string ms_strStreamingAssetBundlePath;

        //AssetBundle Persistent目录
        public static readonly string ms_strPersistentAssetBundlePath;

        //windows 下AssetBundle加载目录
        public static readonly string ms_strWin32AssetBundlePath;

        //AssetBundle 加载目录
        public static readonly string ms_strAssetBundlePath;

        //PC端项目目录
        public static readonly string ms_strWorkingPath = Application.dataPath + "/../";

        static HrResourcePath()
        {
            ms_strProjectplatform = HrAssetBundleUtility.GetPlatformName();
            ms_strAssetBundleOutputPath = Application.dataPath + "/" + STR_ASSETBUNDLES_OUTPUT_PATH;
            ms_strZipAssetBundlePath = Application.streamingAssetsPath + "/" + STR_ZIP_ASSETFILE;
            ms_strZipAssetBundleUnPackPath = Application.persistentDataPath + "/" + "AssetBundles/";


            if (ms_strProjectplatform == "Android")
            {
                ms_strStreamingAssetPathForWWW = Application.streamingAssetsPath + "/";
            }
            else
            {
                ms_strStreamingAssetPathForWWW = "file://" + Application.streamingAssetsPath + "/";
            }
            ms_strPersistentAssetBundlePath = Application.persistentDataPath + "/AssetBundles/" + ms_strProjectplatform + "/";
            ms_strWin32AssetBundlePath = Application.dataPath + "/../../AssetBundles/" + ms_strProjectplatform + "/";
            if (ms_strProjectplatform == "Windows")
            {
                ms_strAssetBundlePath = ms_strWin32AssetBundlePath;
            }
            else
            {
                ms_strAssetBundlePath = ms_strPersistentAssetBundlePath;
            }
        }

        public static string AssetBundlePath
        {
            get { return ms_strAssetBundlePath; }
        }
        
        public static string CombineAssetBundlePath(string assetPath)
        {
            return GetAssetBundlePath() + assetPath;
        }

        public static string GetAssetBundlePath()
        {
            return ms_strAssetBundlePath;
        }

        public static string GetStreamingAssetBundlePath()
        {
            return ms_strStreamingAssetBundlePath;
        }

    }

}



