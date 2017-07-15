using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Resource
{
    public class HrAssetBundle : HrAssetFile
    {
        protected UnityEngine.AssetBundle m_assetBundle;

        protected HrLoadAssetCallBack m_loadDependentCallBack;

        private List<string> m_lisBeDependentOn = new List<string>();
        private List<string> m_lisAssetDependices = new List<string>();
        /// <summary>
        /// 标记已经加载成功的依赖的AssetBundle
        /// </summary>
        private List<string> m_lisLoadedDependentAssetBundle = new List<string>();

        /// <summary>
        /// 资源的依赖资源
        /// </summary>
        public List<string> AsssetDependicesInfo
        {
            get
            {
                return m_lisAssetDependices;
            }
        }

        /// <summary>
        /// 依赖这个AssetBundle的所有AssetBundle
        /// </summary>
        public List<string> BeDependentOnAssetInfo
        {
            get
            {
                return m_lisBeDependentOn;
            }
        }

        /// <summary>
        /// Unity 的AssetBundle
        /// </summary>
        public UnityEngine.AssetBundle MonoAssetBundle
        {
            get { return m_assetBundle; }
        }

        public HrAssetBundle(string strName, string strFullPath) : base(strName, strFullPath)
        {
            m_loadDependentCallBack = new HrLoadAssetCallBack(LoadDependentAssetBundleSuccess, LoadDependentAssetBundleFailed);
        }

        private void LoadDependentAssetBundleSuccess(HrAssetFile assetFile)
        {
            HrLogger.Log(string.Format("when try to load assetbundle '{0}', load dependent asset '{1}' success!", Name, assetFile.Name));
            m_lisLoadedDependentAssetBundle.Add(assetFile.Name);
            
            //因为依赖这个AssetBundle，所以引用计数+1
            assetFile.Retain();
        }

        private void LoadDependentAssetBundleFailed(string strAssetBundleName, string strErrorMsg)
        {
            HrLogger.LogError(string.Format("when try to load assetbundle '{0}', load dependent asset '{1}' failed!", Name, strAssetBundleName));
        }

        public override void LoadSync()
        {
            HrSimpleTimeCounter timeCounter = new HrSimpleTimeCounter();

            m_assetBundleLoadMode = EnumAssetBundleLoadMode.LOAD_SYNC;

            foreach (var strDependice in m_lisAssetDependices)
            {
                HrGameWorld.Instance.ResourceComponent.LoadAssetBundleSync(strDependice, m_loadDependentCallBack);
            }

            this.m_assetBundle = AssetBundle.LoadFromFile(m_strFullPath);

            this.m_assetBundleStatus = EnumAssetBundleStatus.LOADED;
            if (this.LoadAssetBundleEvent != null)
            {
                LoadAssetBundleEvent.TriggerLoadSuccess(this, m_strName, this, timeCounter.GetTimeElapsed());
            }
        }

        public override IEnumerator LoadAsync(HrLoadAssetCallBack loadAssetCallback)
        {
            m_assetBundleLoadMode = EnumAssetBundleLoadMode.LOAD_ASYNC;

            if (AsssetDependicesInfo.Count > 0)
            {
                foreach (var itemAsset in AsssetDependicesInfo)
                {
                    HrGameWorld.Instance.ResourceComponent.LoadAssetBundleAsync(itemAsset, m_loadDependentCallBack);
                }
            }

            AssetBundleCreateRequest  abCreateRequest = AssetBundle.LoadFromFileAsync(m_strFullPath);
            yield return abCreateRequest;

            this.m_assetBundle = abCreateRequest.assetBundle;
            if (this.m_assetBundle == null)
            {
                HrLogger.LogError(string.Format("can not load the asset bundle '{0}' async ", m_strFullPath));

                yield break;
            }

            yield break;
        }

        protected override void ReleaseImp()
        {
            if (IsLoaded() && this.m_assetBundle != null)
            {
                HrLogger.Log(string.Format("release assetbundle '{0}' unload(false)", Name));
                this.m_assetBundle.Unload(false);

                m_assetBundleStatus = EnumAssetBundleStatus.DECLARED;
            }
        }
    }

}
