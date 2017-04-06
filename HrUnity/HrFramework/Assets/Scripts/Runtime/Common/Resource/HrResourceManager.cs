using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using Hr.CommonUtility;


namespace Hr.Resource
{
    public class HrResourceManager : UnitySingleton<HrResourceManager>
    {
        private string mStrVariant = "normal";

        private List<string> mLisAssetBundleName = new List<string>();
        private List<string> mLisAssetBundleWithVariantName = new List<string>();
        private Dictionary<string, List<string>> mDicAssetDependicesInfo = new Dictionary<string, List<string>>();

        private Dictionary<string, HrAssetBundle> mDicAssetBundleInfo = new Dictionary<string, HrAssetBundle>();
        private Dictionary<string, HrAssetBundle> mDicAssetInAssetBundleInfo = new Dictionary<string, HrAssetBundle>();

        public Dictionary<string, HrAssetBundle> AssetBundlePool
        {
            get { return mDicAssetBundleInfo; }
        }

        public string Variant
        {
            set { mStrVariant = value; }
            get { return mStrVariant; }
        }

        public List<string> GetAssetBundleDependices(string strAssetBundleName)
        {
            List<string> lisRt = null;
            mDicAssetDependicesInfo.TryGetValue(strAssetBundleName, out lisRt);

            return lisRt;
        }

        public bool LoadAssetBundleManifest()
        {
            string strAssetBundleMenifestPath = HrAssetBundleUtility.GetAssetBundleManifestPath();
            if (!File.Exists(strAssetBundleMenifestPath))
            {
                Debug.LogError("LoadAssetBundleManifest Error! Is not existed! ManifestPath:" + strAssetBundleMenifestPath);
                return false;
            }
            AssetBundle assetBundle = AssetBundle.LoadFromFile(strAssetBundleMenifestPath);
            AssetBundleManifest manifest = assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            assetBundle.Unload(false);

            var strAllAssetBundles = manifest.GetAllAssetBundles();
            if (mStrVariant.Length > 0)
            {
                strAllAssetBundles = strAllAssetBundles.Where(o => HrFileUtil.GetFileSuffix(o) == mStrVariant).ToArray<string>();
            }
            foreach (var strAssetName in strAllAssetBundles)
            {
                var lisDependicesArr = manifest.GetAllDependencies(strAssetName)
                    .Select(o => HrResourcePath.mStrStreamingAssetBundlePath + o).ToList<string>();
                mDicAssetDependicesInfo.Add(HrResourcePath.mStrStreamingAssetBundlePath + strAssetName, lisDependicesArr);
            }
            mLisAssetBundleName.AddRange(strAllAssetBundles.Select(o => HrResourcePath.mStrStreamingAssetBundlePath + o).ToList<string>());

            var strAllAssetWithVariant = manifest.GetAllAssetBundlesWithVariant();
            if (mStrVariant.Length > 0)
            {
                strAllAssetWithVariant = strAllAssetWithVariant.Where(o => HrFileUtil.GetFileSuffix(o) == mStrVariant)
                    .Select(o => HrResourcePath.mStrStreamingAssetBundlePath + o).ToArray<string>();
            }
            mLisAssetBundleWithVariantName.AddRange(strAllAssetWithVariant);

            return true;
        }

        public void LoadAssetBundleSync(string strAssetBundleName)
        {
            HrAssetBundle loadedAssetBundle = null;
            mDicAssetBundleInfo.TryGetValue(strAssetBundleName, out loadedAssetBundle);
            if (loadedAssetBundle != null)
            {
                while (loadedAssetBundle.IsLoading() && !loadedAssetBundle.IsError()) { }
                
                Debug.LogError("HrResourceManager:LoadAssetBundleSync Error! AssetBundleName:" + strAssetBundleName);
                return;

            }

            if (!File.Exists(strAssetBundleName))
            {
                Debug.LogError("HrResourceManager:LoadAssetBundleSync Error! AssetBunde is not exist!:" + strAssetBundleName);
                return;
            }

            HrAssetBundle assetBundle = new HrAssetBundle(ref strAssetBundleName, new Action<HrAssetBundle>(ActionAssetBundleLoadFinished));
            assetBundle.LoadSync();
        }

        public void ActionAssetBundleLoadFinished(HrAssetBundle assetBundle)
        {
            Debug.Log("ActionAssetBundleLoadFinished AssetBundle:" + assetBundle.Name);

            mDicAssetBundleInfo.Add(assetBundle.Name, assetBundle);

            var strAssetNameArr = assetBundle.MonoAssetBundle.GetAllAssetNames();
            foreach (var strAsset in strAssetNameArr)
            {
                mDicAssetInAssetBundleInfo.Add(strAsset, assetBundle);
            }
        }

        public T GetAsset<T>(string strAssetName) where T : UnityEngine.Object
        {
            HrAssetBundle assetBundleInfo = null;
            mDicAssetInAssetBundleInfo.TryGetValue(strAssetName, out assetBundleInfo);
            if (assetBundleInfo != null)
            {
                return assetBundleInfo.MonoAssetBundle.LoadAsset<T>(strAssetName);
            }
            else
            {
                HrLoger.LogError("HrResourceManager:: GetAsset Error! AssetName:" + strAssetName);
            }

            return null;
        }


    }
}

