using Hr.EventSystem;
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

        private Dictionary<string, Transform> m_dicResourceRoot = new Dictionary<string, Transform>();
        /// <summary>
        /// 资源Instantiate节点
        /// </summary>
        public UnityEngine.Transform ResourceInstanceRoot
        {
            get;
            set;
        }

        /// <summary>
        /// AssetBundle Instantiate节点
        /// </summary>
        public UnityEngine.Transform AssetBundleInstanceRoot
        {
            get;
            set;
        }

        protected override void Awake()
        {
            base.Awake();

            m_resourceManager = HrGameWorld.Instance.GetModule<IResourceManager>();
            if (m_resourceManager != null)
            {
                InitSuccess = true;
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

        public void LoadResourceAsync(int nID, HrLoadResourceCallBack loadResourceCallBack)
        {
            StartCoroutine(LoadResourceAsyncCoroutine(nID, loadResourceCallBack));
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

        public List<HrAssetFile> GetAllAssetFiles()
        {
            return m_resourceManager.GetAllAssetFiles();
        }

        #region private methods

        private IEnumerator LoadResourceAsyncCoroutine(int nID, HrLoadResourceCallBack loadResourceCallBack)
        {
            IEnumerator itor = m_resourceManager.LoadResourceAsync(nID, loadResourceCallBack);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }
        
        #endregion
    }
}
