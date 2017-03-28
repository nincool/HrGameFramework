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

namespace HrCommonUtility
{
    public class HrAssetBundleManager : MonoBehaviour
    {
        public static bool mbLZ4Compression = false;

        public static bool sbLZ4Compression = true;
        public static bool sbLZMACompression = false;
        public static bool sbUnCompression = false;

        public static void LoadAssetBundleManifest()
        {
            string strPlatformName = HrAssetBundleUtility.GetPlatformName();
#if UNITY_EDITOR
            HrLoger.Log(String.Format("LoadAssetBundleManifest PlatformName:{0}", strPlatformName));
#endif
            string strFilePath = Application.streamingAssetsPath + "/" + HrAssetBundleUtility.GetPlatformName() + "/" + HrAssetBundleUtility.GetPlatformName();
            AssetBundle manifestAssetBundle = AssetBundle.LoadFromFile(strFilePath);
            AssetBundleManifest manifest = manifestAssetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            manifestAssetBundle.Unload(false);
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
        

        protected static void LoadAssetBundle(string strAssetBundleName)
        {
#if UNITY_EDITOR
            HrLoger.Log(String.Format("LoadAssetBundle: AssetBundleName[{0}]", strAssetBundleName));
#endif

        }
    }

}