using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using Hr.CommonUtility;
using System.Text.RegularExpressions;

namespace Hr.Resource
{
    public enum EnumHrAssetLoadMode
    {
        STREAMING,
        PERSISTENT,
    }

    public class HrResourceManager : UnitySingleton<HrResourceManager>
    {
        public const EnumHrAssetLoadMode mAssetLoadMode = EnumHrAssetLoadMode.PERSISTENT;

        private string mStrVariant = "";

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
            if (!string.IsNullOrEmpty(mStrVariant))
            {
                strAllAssetBundles = strAllAssetBundles.Where(o => HrFileUtil.GetFileSuffix(o) == mStrVariant).ToArray<string>();
            }
            foreach (var strAssetName in strAllAssetBundles)
            {
                var lisDependicesArr = manifest.GetAllDependencies(strAssetName)
                    .Select(o =>  o.ToLower() ).ToList<string>();
                mDicAssetDependicesInfo.Add(strAssetName, lisDependicesArr);
            }
            mLisAssetBundleName.AddRange(strAllAssetBundles.ToList<string>());

            var strAllAssetWithVariant = manifest.GetAllAssetBundlesWithVariant();
            if (!string.IsNullOrEmpty(mStrVariant))
            {
                strAllAssetWithVariant = strAllAssetWithVariant.Where(o => HrFileUtil.GetFileSuffix(o) == mStrVariant).ToArray<string>();
            }
            mLisAssetBundleWithVariantName.AddRange(strAllAssetWithVariant);

            return true;
        }

        public void LoadAssetBundleSync(string strAssetBundleName)
        {
#if UNITY_EDITOR
            if (Regex.IsMatch(strAssetBundleName, "[A-Z]"))
            {
                HrLoger.LogError("LoadAssetBundleSync Error! AssetBundleName has upper case!");
            }
#endif
            HrAssetBundle loadedAssetBundle = null;
            mDicAssetBundleInfo.TryGetValue(strAssetBundleName, out loadedAssetBundle);
            if (loadedAssetBundle != null)
            {
                while (loadedAssetBundle.IsLoading() && !loadedAssetBundle.IsError()) { }
                
                Debug.LogError("HrResourceManager:LoadAssetBundleSync Error! AssetBundleName:" + strAssetBundleName);
                return;

            }

            string strAssetBundleFullPath = HrResourcePath.CombineAssetBundlePath(strAssetBundleName);

            if (!File.Exists(strAssetBundleFullPath))
            {
                Debug.LogError("HrResourceManager:LoadAssetBundleSync Error! AssetBunde is not exist!:" + strAssetBundleFullPath);
                return;
            }

            HrAssetBundle assetBundle = new HrAssetBundle(strAssetBundleName, strAssetBundleFullPath, new Action<HrAssetBundle>(ActionAssetBundleLoadFinished));
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

