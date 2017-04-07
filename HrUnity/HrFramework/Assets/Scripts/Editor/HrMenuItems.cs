using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using Hr.CommonUtility;

namespace Hr.EditorAssetBundle
{
    public class HrMenuItems
    {
        const string STR_SIMULATION_MODE = "HrTools/Assets/AssetBundles/Simulation Mode";
        const string STR_BUILD_ASSETBUNDLES = "HrTools/Assets/AssetBundles/Build AssetBundles";

        const string STR_BUILD_ASSETBUNDLES_WIN = "HrTools/Assets/AssetBundles/Build AssetBundles";
        const string STR_SERIALIZE_DATA_WIN = "HrTools/Assets/AssetBundles/Serialize Data";

        const string STR_ZIP_ASSET = "HrTools/Assets/AssetBundles/Zip File";

        const string STR_PRINT_MONO_VERSION = "HrTools/PrintMono Version";

        const string STR_HR_TEST = "HrTools/HrTestButton";

        [MenuItem(STR_SIMULATION_MODE)]
        public static void ToggleSimulationMode()
        {
            
        }

        [MenuItem(STR_SIMULATION_MODE, true)]
        public static bool ToggleSimulationModeValidate()
        {
            Menu.SetChecked(STR_SIMULATION_MODE, true);
            return true;
        }

        [MenuItem(STR_BUILD_ASSETBUNDLES)]
        public static void BuildAssetBundles()
        {
            HrBuildAssetBundle.BuildAssetBundles();
        }

        [MenuItem(STR_BUILD_ASSETBUNDLES_WIN, false, 100)]
        public static void OpenBuildAssetBundlesWin()
        {
            HrBuildAssetBundleWin.OpenBuildAssetBundleWin();
        }

        [MenuItem(STR_SERIALIZE_DATA_WIN, false, 101)]
        public static void OpenSerializeDataWin()
        {
            HrSerializeDataWin.OpenSerializeDataWin();
        }

        [MenuItem(STR_ZIP_ASSET, false, 102)]
        public static void OpenZipFileWin()
        {
            HrZipFileWin.OpenZipFileWin();
        }

        [MenuItem(STR_PRINT_MONO_VERSION)]
        public static void PrintMonoVersion()
        {
            Type type = Type.GetType("Mono.Runtime");
            if (type != null)
            {
                MethodInfo displayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
                if (displayName != null)
                    Debug.Log(displayName.Invoke(null, null));
            }
        }

        [MenuItem(STR_HR_TEST)]
        public static void HrTestButton()
        {
            HrZipFileUtil.PackFiles("E:\\Workspace\\HrGitHub\\HrGameFramework\\HrUnity\\HrFramework\\Assets\\123.zip", "E:\\Workspace\\HrGitHub\\HrGameFramework\\HrUnity\\HrFramework\\Assets\\AssetBundles");
        }
    }
}
