using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using Hr.CommonUtility;

namespace Hr.Resource
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
        private string mStrName;
        private string mStrFullName;

        private AssetBundle mAssetBundle;

        private Action<HrAssetBundle> mLoadedAction;

        //当前资源状态
        private EnumHrAssetBundleStatus mAssetBundleStatus = EnumHrAssetBundleStatus.UNDEFINED;

        public HrAssetBundle(string strName, string strFullName,  Action<HrAssetBundle> loadedAction)
        {
            mStrName = strName;
            mStrFullName = strFullName;

            mAssetBundleStatus = EnumHrAssetBundleStatus.DECLARED;
            mLoadedAction = loadedAction;
        }

        public string Name
        {
            set { mStrName = value; }
            get { return mStrName; }
        }

        public string FullName
        {
            set { mStrFullName = value; }
            get { return mStrFullName; }
        }

        public EnumHrAssetBundleStatus AssetBundleStatus
        {
            set { mAssetBundleStatus = value; }
            get { return mAssetBundleStatus; }
        }

        public AssetBundle MonoAssetBundle
        {
            get { return mAssetBundle; }
        }

        public bool IsLoading()
        {
            return (mAssetBundleStatus == EnumHrAssetBundleStatus.LOADING);
        }

        public bool IsLoaded()
        {
            return (mAssetBundleStatus == EnumHrAssetBundleStatus.LOADED);
        }

        public bool IsError()
        {
            return false;
        }

        public void LoadSync()
        {
            var lisDependices = HrResourceManager.Instance.GetAssetBundleDependices(mStrName);

            //遍历加载依赖资源
            foreach (var itemAsset in lisDependices)
            {
                HrResourceManager.Instance.LoadAssetBundleSync(itemAsset);
            }

            this.mAssetBundle = AssetBundle.LoadFromFile(mStrFullName);

            this.mAssetBundleStatus = EnumHrAssetBundleStatus.LOADED;

            if (this.mLoadedAction != null)
            {
                this.mLoadedAction(this);
            }
        }

        public IEnumerator LoadAsync()
        {
            yield break;
        }
    }
}