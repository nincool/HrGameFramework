using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr;
using System;

namespace Hr.Resource
{
    public class HrResourcePath
    {
        public const string STR_ZIP_ASSETFILE = "assets.zip";

        /// <summary>
        /// 平台名称
        /// </summary>
        public static readonly string m_s_strProjectplatform;

        /// <summary>
        /// PC AssetBundle所在目录
        /// </summary>
        public static readonly string m_s_strAssetBundleOutputPath;

        /// <summary>
        ///  初始化安装包 AssetBundle压缩为zip存放目录 
        /// </summary>
        public static readonly string m_s_strZipAssetBundlePath;

        /// <summary>
        /// zip包解压缩目录
        /// </summary>
        public static readonly string m_s_strZipAssetBundleUnPackPath;

        /// <summary>
        /// StreamingAsset www加载路径
        /// </summary>
        public static readonly string m_s_strStreamingAssetPathForWWW;

        /// <summary>
        /// AssetBundle Streaming目录 目前不用只放初始Zip包
        /// </summary>
        public static readonly string m_s_strStreamingAssetBundlePath;

        /// <summary>
        /// AssetBundle Persistent目录
        /// </summary>
        public static readonly string m_s_strPersistentAssetBundlePath;

        /// <summary>
        /// windows 下AssetBundle加载目录
        /// </summary>
        public static readonly string m_s_strWin32AssetBundlePath;

        /// <summary>
        /// AssetBundle 加载目录
        /// </summary>
        public static readonly string m_s_strAssetBundlePath;

        //PC端项目目录
        public static readonly string ms_strWorkingPath = Application.dataPath + "/../";

        static HrResourcePath()
        {
            m_s_strProjectplatform = HrAssetBundleUtility.GetPlatformName();

            m_s_strZipAssetBundlePath = Application.streamingAssetsPath + "/" + STR_ZIP_ASSETFILE;

            if (m_s_strProjectplatform.Equals("android", StringComparison.OrdinalIgnoreCase))
            {
                m_s_strStreamingAssetPathForWWW = Application.streamingAssetsPath + "/";
            }
            else
            {
                m_s_strStreamingAssetPathForWWW = "file://" + Application.streamingAssetsPath + "/";
            }

            m_s_strPersistentAssetBundlePath = Application.persistentDataPath + "/AssetBundles/" + m_s_strProjectplatform + "/";
            m_s_strWin32AssetBundlePath = Application.dataPath + "/../../AssetBundles/AssetBundles/" + m_s_strProjectplatform + "/";

            if (m_s_strProjectplatform.Equals("windows", StringComparison.OrdinalIgnoreCase))
            {
                m_s_strAssetBundlePath = m_s_strWin32AssetBundlePath;
                m_s_strZipAssetBundleUnPackPath = Application.persistentDataPath + "/../../AssetBundles/";
            }
            else
            {
                m_s_strAssetBundlePath = Application.persistentDataPath + "/AssetBundles/";
                m_s_strZipAssetBundleUnPackPath = Application.persistentDataPath + "/AssetBundles/";
            }
        }

        public static string AssetBundlePath
        {
            get { return m_s_strAssetBundlePath; }
        }
        
        public static string CombineAssetBundlePath(string assetPath)
        {
            return GetAssetBundlePath() + assetPath;
        }

        public static string GetAssetBundlePath()
        {
            return m_s_strAssetBundlePath;
        }

        public static string GetStreamingAssetBundlePath()
        {
            return m_s_strStreamingAssetBundlePath;
        }

    }

}



