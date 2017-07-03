using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Resource
{
    public class HrResourceScene : HrResource
    {
        public HrResourceScene(string strAssetName, HrAssetFile assetFile) : base(strAssetName, assetFile)
        {

        }

        public static void RegisterType()
        {
            HrResourceManager.AddResourceType(typeof(UnityEngine.GameObject), typeof(HrResourceScene));
        }


    }

}
