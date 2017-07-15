using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Resource
{
    public class HrResourceScene : HrResource
    {
        public HrResourceScene(string strAssetName, UnityEngine.Object o, HrAssetFile assetFile) : base(strAssetName, o, assetFile)
        {

        }

        public static void RegisterType()
        {
            HrResourceManager.AddResourceType(typeof(UnityEngine.GameObject), typeof(HrResourceScene));
        }


    }

}
