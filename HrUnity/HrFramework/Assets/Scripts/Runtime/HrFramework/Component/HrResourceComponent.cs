using Hr.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrResourceComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private IResourceManager m_resourceManager;

        protected override void Awake()
        {
            base.Awake();

            m_resourceManager = HrGameWorld.Instance.GetModule<HrResourceManager>();
            if (m_resourceManager != null)
            {
                InitSuccess = true;
            }
        }

        protected override void Start()
        {
            base.Start();

        }

        public void LoadSceneSync(string strSceneName, string strAssetBundleName)
        {
            m_resourceManager.LoadSceneSync(strSceneName, strAssetBundleName);
        }

        //public List<string> GetAssetBundleDependices(string strAssetBundleName)
        //{
        //    return m_resourceManager.GetAssetBundleDependices(strAssetBundleName);
        //}

        //public HrAssetBundle LoadAssetBundleSync(string strAssetBundleName)
        //{
        //    return m_resourceManager.LoadAssetBundleSync(strAssetBundleName);
        //}

        //public T LoadAsset<T>(string strAssetPath)
        //{
        //    return m_resourceManager.LoadAsset<T>(strAssetPath);
        //}
    }

}
