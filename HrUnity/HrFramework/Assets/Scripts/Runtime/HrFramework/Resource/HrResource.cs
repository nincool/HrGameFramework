﻿using Hr;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

namespace Hr
{
    public class HrResource
    {
        private string m_strAssetName;

        private WeakReference m_weakRefAssetBundle;

        public string AssetName
        {
            get { return m_strAssetName; }
        }

        public HrAssetBundle RefAssetBundle
        {
            get
            {
                if (!m_weakRefAssetBundle.IsAlive)
                {
                    return null;
                }
                return m_weakRefAssetBundle.Target as HrAssetBundle;
            }
        }

        public HrResource(string strAssetName, HrAssetBundle assetBundle)
        {
            m_strAssetName = strAssetName;
            m_weakRefAssetBundle = new WeakReference(assetBundle);
        }
    }

}

