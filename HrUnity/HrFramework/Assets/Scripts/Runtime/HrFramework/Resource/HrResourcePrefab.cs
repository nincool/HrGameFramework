using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrResourcePrefab : HrResource
    {
        public HrResourcePrefab(string strAssetName, UnityEngine.Object o, HrAssetBundle assetBundle) : base(strAssetName, o, assetBundle)
        {
        }

        public static void RegisterType()
        {
            HrResourceManager.AddResourceType(typeof(UnityEngine.GameObject), typeof(HrResourcePrefab));
        }

    }
}
