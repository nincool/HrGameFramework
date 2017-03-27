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
        public static void LoadAssetBundleManifest()
        {
            string strPlatformName = HrAssetBundleUtility.GetPlatformName();
#if UNITY_EDITOR
            HrLoger.Log(String.Format("LoadAssetBundleManifest PlatformName:{0}", strPlatformName));
#endif
            string strFilePath =  HrAssetBundleUtility.GetPlatformName();
            strFilePath = strFilePath + "/hrassets/" + "hrtestassetbundle01";
            HrLoger.Log("FileName:" + strFilePath);
            UnityEngine.Object obj = AssetBundle.LoadFromFile(strFilePath);
            AssetBundleManifest mainManifest = obj as AssetBundleManifest;
            string[] strAllAssetBundles = mainManifest.GetAllAssetBundles();
            for (var i = 0; i < strAllAssetBundles.Length; ++i)
            {
                HrLoger.Log(strAllAssetBundles[i]);
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