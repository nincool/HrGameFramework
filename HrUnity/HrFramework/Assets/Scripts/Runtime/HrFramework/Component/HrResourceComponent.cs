using Hr.EventSystem;
using Hr.Resource;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrResourceComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private IResourceManager m_resourceManager;

        private Dictionary<string, Transform> m_dicResourceRoot = new Dictionary<string, Transform>();
        /// <summary>
        /// 资源Instantiate节点
        /// </summary>
        public UnityEngine.Transform ResourceInstanceRoot
        {
            get;
            set;
        }

        protected override void Awake()
        {
            base.Awake();

            m_resourceManager = HrGameWorld.Instance.GetModule<HrResourceManager>();
            if (m_resourceManager != null)
            {
                InitSuccess = true;

                HrGameWorld.Instance.GetModule<HrEventManager>();
            }
            else
            {
                throw new HrException("HrResourceComponent init error!");
            }
        }

        protected override void Start()
        {
            base.Start();

            if (ResourceInstanceRoot == null)
            {
                ResourceInstanceRoot = (new GameObject("Resource Instances")).transform;
                ResourceInstanceRoot.SetParent(gameObject.transform);
                ResourceInstanceRoot.localScale = Vector3.one;
            }
        }

        public void LoadAssetsConfig()
        {
            m_resourceManager.LoadAssetsConfig();
        }

        public void LoadResourceSync(int nID, HrLoadResourceCallBack loadResourceCallBack)
        {
            m_resourceManager.LoadResourceSync(nID, loadResourceCallBack);
        }

        public void LoadAssetBundleSync(string strAssetBundleName, HrLoadAssetCallBack loadAssetCallback)
        {
            m_resourceManager.LoadAssetBundleSync(strAssetBundleName, loadAssetCallback);
        }

        public void LoadAssetBundleAsync(string strAssetBundleName, HrLoadAssetCallBack loadAssetCallback)
        {
            m_resourceManager.LoadAssetBundleAsync(strAssetBundleName, loadAssetCallback);
        }

        public void LoadAssetBundleWithFullPathSync(string strFullPath, HrLoadAssetCallBack loadAssetCallBack)
        {
            m_resourceManager.LoadAssetBundleWithFullPathSync(strFullPath, loadAssetCallBack);
        }

        public HrResource GetResource(int nID)
        {
            return m_resourceManager.GetResource(nID);
        }

        public HrResource GetResource(string strResourceName)
        {
            return m_resourceManager.GetResource(strResourceName);
        }

        public void AddResourceGameObject<T>(Transform trans)
        {
            var resRoot = m_dicResourceRoot.HrTryGet(typeof(T).FullName);
            if (resRoot == null)
            {
                resRoot = (new GameObject(typeof(T).FullName)).transform;
                resRoot.parent = ResourceInstanceRoot;
                resRoot.localScale = Vector3.one;
                trans.SetParent(resRoot);
                trans.localScale = Vector3.one;
                trans.gameObject.SetActive(false);
            }
        }

        //public HrResource GetResource(int nID)
        //{
        //    return m_resourceManager.GetResource(nID);
        //}

        //public List<string> GetAssetBundleDependices(string strAssetBundleName)
        //{
        //    return m_resourceManager.GetAssetBundleDependices(strAssetBundleName);
        //}

        //public HrAssetFile LoadAssetBundleSync(string strAssetBundleName)
        //{
        //    return m_resourceManager.LoadAssetBundleSync(strAssetBundleName);
        //}

        //public T LoadAsset<T>(string strAssetPath)
        //{
        //    return m_resourceManager.LoadAsset<T>(strAssetPath);
        //}
    }

}
