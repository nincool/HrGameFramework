using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrResourcePrefab : HrResource
    {
        public HrResourcePrefab(string strAssetName, HrAssetBundle assetBundle) : base(strAssetName, assetBundle)
        {
        }

        public static void RegisterType()
        {
            HrResourceManager.AddResourceType(typeof(UnityEngine.GameObject), typeof(HrResourcePrefab));
        }

    }
}
