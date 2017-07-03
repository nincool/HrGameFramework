using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr;
using System;

namespace Hr.Resource
{
    public class HrResourcePath
    {
        /// <summary>
        /// 平台名称
        /// </summary>
        public static readonly string m_s_strProjectplatform;

        /// <summary>
        /// StreamingAsset www加载路径
        /// </summary>
        public static readonly string m_s_strStreamingAssetPathForWWW;

        /// <summary>
        /// 资源目录
        /// </summary>
        public static readonly string m_s_strResourcePath;

        /// <summary>
        /// AssetBundle 加载目录
        /// </summary>
        public static readonly string m_s_strAssetBundlePath;

        /// <summary>
        /// DataTable 加载目录 
        /// </summary>
        public static readonly string m_s_strDataTablePath;

        static HrResourcePath()
        {
            m_s_strProjectplatform = HrAssetBundleUtility.GetPlatformName();


            //if (m_s_strProjectplatform.Equals("android", StringComparison.OrdinalIgnoreCase))
            //{
            //    m_s_strStreamingAssetPathForWWW = Application.streamingAssetsPath + "/";
            //}
            //else
            //{
            //    m_s_strStreamingAssetPathForWWW = "file://" + Application.streamingAssetsPath + "/";
            //}
            m_s_strStreamingAssetPathForWWW = Application.streamingAssetsPath + "/";

            if (m_s_strProjectplatform.Equals("windows", StringComparison.OrdinalIgnoreCase))
            {
                m_s_strResourcePath = Application.dataPath + "/../../Resource/";
                m_s_strAssetBundlePath = ResourcePath + "AssetBundles/" + m_s_strProjectplatform + "/";
                m_s_strDataTablePath = ResourcePath + "DataTable/";
            }
            else
            {
                m_s_strAssetBundlePath = Application.persistentDataPath + "/AssetBundles/";
                m_s_strDataTablePath = Application.persistentDataPath + "/DataTable/";
            }
        }

        public static string ResourcePath
        {
            get
            {
                return m_s_strResourcePath;
            }
        }

        public static string AssetBundlePath
        {
            get
            {
                return m_s_strAssetBundlePath;
            }
        }
        
        public static string DataTablePath
        {
            get
            {
                return m_s_strDataTablePath;
            }
        }

        public static string CombineAssetBundlePath(string assetPath)
        {
            return AssetBundlePath + assetPath;
        }

        public static string CombineDataTablePath(string strDataTableName)
        {
            return DataTablePath + strDataTableName;
        }

        public static string CombineStreamingAssetsPath(string strAssetPath)
        {
            return m_s_strStreamingAssetPathForWWW + strAssetPath;
        }

        public static string GetAssetBundleAssetsListFilePath()
        {
            return ResourcePath + "AssetBundles/AssetsList.json";
        }

        public static string GetDataTableConfigFilePath()
        {
            return ResourcePath + "DataTable/DataTableConfig.json";
        }
    }

}



