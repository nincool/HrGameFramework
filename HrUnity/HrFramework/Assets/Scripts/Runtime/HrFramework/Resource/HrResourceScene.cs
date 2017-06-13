using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Resource
{
    public class HrResourceScene : HrResource
    {
        public HrResourceScene(string strAssetName, UnityEngine.Object o, HrAssetBundle assetBundle) : base(strAssetName, o, assetBundle)
        {

        }

        public static void RegisterType()
        {
            HrResourceManager.AddResourceType(typeof(UnityEngine.GameObject), typeof(HrResourceScene));
        }


    }

}
