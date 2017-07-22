using Hr.ReleasePool;
using System.Collections;

namespace Hr.Resource
{
    public enum EnumAssetBundleStatus
    {
        UNDEFINED,
        DECLARED,
        LOADING,
        LOADED,
    }

    public enum EnumAssetBundleLoadMode
    {
        LOAD_SYNC,
        LOAD_ASYNC
    }

    public abstract class HrAssetFile : HrRef, IAssetLoad
    {
        protected string m_strName;

        protected string m_strFullPath;

        protected HrReleaseStrategy m_ReleaseStrategy;
        /// <summary>
        /// 加载资源事件
        /// </summary>
        protected HrLoadAssetEvent m_loadAssetBundleEvent = new HrLoadAssetEvent();

        public HrLoadAssetEvent LoadAssetBundleEvent
        {
            get { return m_loadAssetBundleEvent; }
        }

        /// <summary>
        /// 当前的资源状态
        /// </summary>
        protected EnumAssetBundleStatus m_assetBundleStatus = EnumAssetBundleStatus.UNDEFINED;

        /// <summary>
        /// 资源加载模式
        /// </summary>
        protected EnumAssetBundleLoadMode m_assetBundleLoadMode = EnumAssetBundleLoadMode.LOAD_SYNC;

        protected bool m_bIsError = false;

        public HrAssetFile(string strName, string strFullPath)
        {
            m_strName = strName;
            m_strFullPath = strFullPath;

            m_assetBundleStatus = EnumAssetBundleStatus.DECLARED;

            m_ReleaseStrategy = new HrReleaseStrategy(this);
        }

        public string Name
        {
            set { m_strName = value; }
            get { return m_strName; }
        }

        public string FullPath
        {
            set { m_strFullPath = value; }
            get { return m_strFullPath; }
        }

        public EnumAssetBundleStatus AssetBundleStatus
        {
            set { m_assetBundleStatus = value; }
            get { return m_assetBundleStatus; }
        }


        public HrReleaseStrategy ReleaseStrategy
        {
            get { return m_ReleaseStrategy; }
        }

        public bool IsLoading()
        {
            return (m_assetBundleStatus == EnumAssetBundleStatus.LOADING);
        }

        public bool IsLoaded()
        {
            return (m_assetBundleStatus == EnumAssetBundleStatus.LOADED);
        }

        public bool IsError()
        {
            return m_bIsError;
        }

        public abstract void LoadSync();


        public abstract IEnumerator LoadAsync();

        public void Update(float fElapseSeconds, float fRealElapseSeconds)
        {
            if (IsLoaded())
            {
                m_ReleaseStrategy.Update(fElapseSeconds, fRealElapseSeconds);
            }
        }

        public void AutoRelease()
        {
            HrGameWorld.Instance.ReleasePoolComonent.AddReleaseStrategyObject(m_ReleaseStrategy);
        }
    }
}