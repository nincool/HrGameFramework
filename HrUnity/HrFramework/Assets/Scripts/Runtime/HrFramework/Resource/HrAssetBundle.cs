using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using Hr;

namespace Hr
{
    public enum EnumHrAssetBundleStatus
    {
        UNDEFINED,
        DECLARED,
        LOADING,
        LOADED,
    }

    public class HrAssetBundle : HrRef, IAssetLoad
    {
        //AssetBundle Name
        private string m_strName;
        private string m_strFullName;

        private AssetBundle m_assetBundle;

        private Action<HrAssetBundle> m_actLoaded;

        //当前资源状态
        private EnumHrAssetBundleStatus m_assetBundleStatus = EnumHrAssetBundleStatus.UNDEFINED;

        public HrAssetBundle(string strName, string strFullName,  Action<HrAssetBundle> loadedAction)
        {
            m_strName = strName;
            m_strFullName = strFullName;

            m_assetBundleStatus = EnumHrAssetBundleStatus.DECLARED;
            m_actLoaded = loadedAction;
        }

        public string Name
        {
            set { m_strName = value; }
            get { return m_strName; }
        }

        public string FullName
        {
            set { m_strFullName = value; }
            get { return m_strFullName; }
        }

        public EnumHrAssetBundleStatus AssetBundleStatus
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
            return (m_assetBundleStatus == EnumHrAssetBundleStatus.LOADING);
        }

        public bool IsLoaded()
        {
            return (m_assetBundleStatus == EnumHrAssetBundleStatus.LOADED);
        }

        public bool IsError()
        {
            return false;
        }

        public void LoadSync()
        {
            var lisDependices = HrResourceManager.Instance.GetAssetBundleDependices(m_strName);
            if (lisDependices != null && lisDependices.Count > 0)
            {
                //遍历加载依赖资源
                foreach (var itemAsset in lisDependices)
                {
                    HrResourceManager.Instance.LoadAssetBundleSync(itemAsset);
                }
            }

            this.m_assetBundle = AssetBundle.LoadFromFile(m_strFullName);

            this.m_assetBundleStatus = EnumHrAssetBundleStatus.LOADED;

            if (this.m_actLoaded != null)
            {
                this.m_actLoaded(this);
            }
        }

        public IEnumerator LoadAsync()
        {
            yield break;
        }
    }
}