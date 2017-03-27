using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HrCommonUtility
{
    public class HrAssetBundleLoadAssets : MonoBehaviour
    {
        private void Awake()
        {
            HrAssetBundleManager.LoadAssetBundleManifest();
        }
    }
}