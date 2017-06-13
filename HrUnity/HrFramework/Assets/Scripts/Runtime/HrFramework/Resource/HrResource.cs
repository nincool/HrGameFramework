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
        public UnityEngine.Object m_unityAsset;

        /// <summary>
        /// 资源名称 对应的Assets目录下名称
        /// </summary>
        private string m_strAssetName;

        /// <summary>
        /// 对AssetBundle的弱引用
        /// </summary>
        private WeakReference m_weakRefAssetBundle;

        public string AssetName
        {
            get { return m_strAssetName; }
        }

        public HrAssetBundle RefAssetBundle
        {
            get
            {
                if (m_weakRefAssetBundle == null || !m_weakRefAssetBundle.IsAlive)
                {
                    return null;
                }
                return m_weakRefAssetBundle.Target as HrAssetBundle;
            }
        }

        public HrResource(string strAssetName, UnityEngine.Object o, HrAssetBundle assetBundle)
        {
            m_strAssetName = strAssetName;
            m_unityAsset = o;
            if (assetBundle != null)
                m_weakRefAssetBundle = new WeakReference(assetBundle);
        }
    }

}

