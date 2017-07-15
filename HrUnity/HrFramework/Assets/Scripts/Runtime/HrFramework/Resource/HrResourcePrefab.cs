using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Resource
{
    public class HrResourcePrefab : HrResource
    {
        public UnityEngine.GameObject InstancePrefab
        {
            get;
            private set;
        }

        public HrResourcePrefab(string strAssetName, UnityEngine.Object o, HrAssetFile assetFile) : base(strAssetName, o, assetFile)
        {
            InstancePrefab = m_unityAsset as GameObject;
            
            //PrefabInstantiate();
        }

        public static void RegisterType()
        {
            HrResourceManager.AddResourceType(typeof(UnityEngine.GameObject), typeof(HrResourcePrefab));
        }

        /// <summary>
        /// 测试函数 加载显示 Instance会导致绑定的Component执行Awake
        /// </summary>
        private void PrefabInstantiate()
        {
            HrGameWorld.Instance.ResourceComponent.AddResourceGameObject<HrResourcePrefab>(InstancePrefab.transform);
        }
    }
}
