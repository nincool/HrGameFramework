using Hr.Resource;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public class HrBuildAssetBundle
    {
        static public string CreateAssetBundleDirectory()
        {
            // Choose the output path according to the build target.
            string strOutputPath = "";// Application.dataPath + "/" + HrResourcePath.STR_ASSETBUNDLES_OUTPUT_PATH + HrAssetBundleUtility.GetPlatformName();
            //if (!Directory.Exists(strOutputPath))
            //    Directory.CreateDirectory(strOutputPath);

            return strOutputPath;
        }

        public static void BuildAssetBundles()
        {
            BuildAssetBundles(null, null);
        }

        public static void BuildAssetBundles(AssetBundleBuild[] builds, string strPath)
        {
            // Choose the output path according to the build target.
            string strOutputPath = strPath;
            if (string.IsNullOrEmpty(strOutputPath))
            {
                strOutputPath = CreateAssetBundleDirectory();
            }

            var options = BuildAssetBundleOptions.None;

            bool shouldCheckODR = EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS;
#if UNITY_TVOS
            shouldCheckODR |= EditorUserBuildSettings.activeBuildTarget == BuildTarget.tvOS;
#endif
            if (shouldCheckODR)
            {
#if ENABLE_IOS_ON_DEMAND_RESOURCES
                if (PlayerSettings.iOS.useOnDemandResources)
                    options |= BuildAssetBundleOptions.UncompressedAssetBundle;
#endif
#if ENABLE_IOS_APP_SLICING
                options |= BuildAssetBundleOptions.UncompressedAssetBundle;
#endif
            }
            else
            {
                if (HrBuildAssetBundleWin.sbLZMACompression)
                    options = BuildAssetBundleOptions.None;
                if (HrBuildAssetBundleWin.sbLZ4Compression)
                    options = BuildAssetBundleOptions.ChunkBasedCompression;
                if (HrBuildAssetBundleWin.sbUnCompression)
                    options = BuildAssetBundleOptions.UncompressedAssetBundle;
            }

            if (builds == null || builds.Length == 0)
            {
                //@TODO: use append hash... (Make sure pipeline works correctly with it.)
                BuildPipeline.BuildAssetBundles(strOutputPath, options, EditorUserBuildSettings.activeBuildTarget);
            }
            else
            {
                BuildPipeline.BuildAssetBundles(strOutputPath, builds, options, EditorUserBuildSettings.activeBuildTarget);
            }
        }

    }
}


