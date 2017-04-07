using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/*  The AssetBundle Manager provides a High-Level API for working with AssetBundles. 
	The AssetBundle Manager will take care of loading AssetBundles and their associated 
	Asset Dependencies.
		Initialize()
			Initializes the AssetBundle manifest object.
		LoadAssetAsync()
			Loads a given asset from a given AssetBundle and handles all the dependencies.
		LoadLevelAsync()
			Loads a given scene from a given AssetBundle and handles all the dependencies.
		LoadDependencies()
			Loads all the dependent AssetBundles for a given AssetBundle.
		BaseDownloadingURL
			Sets the base downloading url which is used for automatic downloading dependencies.
		SimulateAssetBundleInEditor
			Sets Simulation Mode in the Editor.
		Variants
			Sets the active variant.
		RemapVariantName()
			Resolves the correct AssetBundle according to the active variant.
*/

namespace Hr.CommonUtility
{
    public class HrAssetBundleManager : MonoBehaviour
    {
        public static bool mbLZ4Compression = false;

        public static void LoadAssetBundleManifest()
        {
            string strPlatformName = HrAssetBundleUtility.GetPlatformName();
#if UNITY_EDITOR
            HrLoger.Log(String.Format("LoadAssetBundleManifest PlatformName:{0}", strPlatformName));
#endif
            string strFilePath = Application.streamingAssetsPath + "/" + strPlatformName + "/" + strPlatformName;
            AssetBundle assetBundle = AssetBundle.LoadFromFile(strFilePath);
            AssetBundleManifest manifest = assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            assetBundle.Unload(false);
            string[] strAllAssetBundles = manifest.GetAllAssetBundles();
            foreach (var item in strAllAssetBundles)
            {
                HrLoger.Log(item);
            }
            string[] strAllAssetWithVariant = manifest.GetAllAssetBundlesWithVariant();
            foreach (var item in strAllAssetWithVariant)
            {
                HrLoger.Log(item);
            }
        }

        protected static void LoadAssetBundle(string strAssetBundlePath)
        {
            string strPlatformName = HrAssetBundleUtility.GetPlatformName();
#if UNITY_EDITOR
            HrLoger.Log(String.Format("LoadAssetBundle: AssetBundleName[{0}] PlatformName[{1}]", strAssetBundlePath, strPlatformName));
#endif
            string strFilePath = Application.streamingAssetsPath + "/" + strPlatformName + "/" + strAssetBundlePath;
            if (Directory.Exists(strFilePath))
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(strFilePath);
                AssetBundleManifest manifest = assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;

                string[] strAllAssetBundles = manifest.GetAllAssetBundles();
                foreach (var strItemAsset in strAllAssetBundles)
                {
                    string[] strAllDependicesArr = manifest.GetAllDependencies(strItemAsset);
                    foreach (var strDependAssetBundle in strAllDependicesArr)
                    {
                        LoadAssetBundle(strDependAssetBundle);
                    }
                }

                var assetObjArr = assetBundle.LoadAllAssets();

            }
            else
            {
                HrLoger.LogError(String.Format("LoadAssetBundle Error! AssetBundlePath[{0}]", strAssetBundlePath));
            }

        }
    }

}