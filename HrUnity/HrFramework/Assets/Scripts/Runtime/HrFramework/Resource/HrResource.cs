using Hr;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

namespace Hr.Resource
{
    public class HrResource
    {
        /// <summary>
        /// Unity 资源对象
        /// </summary>
        protected UnityEngine.Object m_unityAsset;

        /// <summary>
        /// 资源名称 对应的Assets目录下名称
        /// </summary>
        private string m_strAssetName;

        /// <summary>
        /// 对AssetBundle的弱引用
        /// </summary>
        protected WeakReference m_weakRefAssetFile;

        public string AssetName
        {
            get { return m_strAssetName; }
        }

        public UnityEngine.Object UnityAsset
        {
            get
            {
                return m_unityAsset;
            }
        }

        public HrAssetFile RefAssetFile
        {
            get
            {
                if (m_weakRefAssetFile == null || !m_weakRefAssetFile.IsAlive)
                {
                    return null;
                }
                return m_weakRefAssetFile.Target as HrAssetFile;
            }
        }

        public HrResource(string strAssetName, UnityEngine.Object o, HrAssetFile assetFile)
        {
            m_strAssetName = strAssetName;
            m_unityAsset = o;
            if (assetFile != null)
                m_weakRefAssetFile = new WeakReference(assetFile);
        }
    }

}

