using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using Hr;

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

    public class HrAssetBundle : HrRef, IAssetLoad
    {
        private string m_strName;
        
        private string m_strFullPath;

        private AssetBundle m_assetBundle;

        /// <summary>
        /// 加载资源事件
        /// </summary>
        private HrLoadAssetEvent m_loadAssetBundleEvent = new HrLoadAssetEvent();

        public HrLoadAssetEvent LoadAssetBundleEvent
        {
            get { return m_loadAssetBundleEvent; }
        }

        /// <summary>
        /// 当前的资源状态
        /// </summary>
        private EnumAssetBundleStatus m_assetBundleStatus = EnumAssetBundleStatus.UNDEFINED;

        /// <summary>
        /// 
        /// </summary>
        private EnumAssetBundleLoadMode m_assetBundleLoadMode = EnumAssetBundleLoadMode.LOAD_SYNC;

        public HrAssetBundle(string strName, string strFullPath)
        {
            m_strName = strName;
            m_strFullPath = strFullPath;

            m_assetBundleStatus = EnumAssetBundleStatus.DECLARED;
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

        public AssetBundle MonoAssetBundle
        {
            get { return m_assetBundle; }
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
            return false;
        }

        public void LoadSync()
        {
            HrSimpleTimeCounter timeCounter = new HrSimpleTimeCounter();

            m_assetBundleLoadMode = EnumAssetBundleLoadMode.LOAD_SYNC;

            var lisDependices = new List<string>();// = HrGameWorld.Instance.ResourceComponent.GetAssetBundleDependices(m_strName);
            if (lisDependices != null && lisDependices.Count > 0)
            {
                //遍历加载依赖资源
                foreach (var itemAsset in lisDependices)
                {
                    //HrGameWorld.Instance.ResourceComponent.LoadAssetBundleSync(itemAsset);
                }
            }

            this.m_assetBundle = AssetBundle.LoadFromFile(m_strFullPath);

            this.m_assetBundleStatus = EnumAssetBundleStatus.LOADED;

            if (this.LoadAssetBundleEvent != null)
            {
                LoadAssetBundleEvent.TriggerLoadSuccess(this, m_strName, this, timeCounter.GetTimeElapsed());
            }
        }

        public IEnumerator LoadAsync()
        {
            yield break;
        }
    }
}