using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Resource
{
    public class HrResourcePrefab : HrResource
    {
        public HrResourcePrefab(string strAssetName, HrAssetFile assetFile) : base(strAssetName, assetFile)
        {
        }

        public static void RegisterType()
        {
            HrResourceManager.AddResourceType(typeof(UnityEngine.GameObject), typeof(HrResourcePrefab));
        }

    }
}
