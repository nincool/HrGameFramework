using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using Hr;

namespace Hr.Editor
{
    public class HrMenuItems
    {

        const string STR_SIMULATION_MODE = "HrTools/Assets/AssetBundles/Simulation Mode";

        //AssetBundle界面
        const string STR_BUILD_ASSETBUNDLES_WIN = "HrTools/Assets/AssetBundles/Build AssetBundles";
        //预览AssetBundle
        const string STR_PREVIEW_ASSETBUNDLE_WIN = "HrTools/Assets/AssetBundles/Preview AssetBundle";
        
        //序列化界面
        const string STR_SERIALIZE_DATA_WIN = "HrTools/Assets/AssetBundles/Serialize Data";
        //压缩zip界面
        const string STR_ZIP_ASSET = "HrTools/Assets/AssetBundles/Zip File";
        //UGUI Atlas界面 
        const string STR_ATLAS_TOOL_WIN = "HrTools/Assets/Atlas Maker";
        
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

        [MenuItem(STR_PREVIEW_ASSETBUNDLE_WIN, false, 103)]
        public static void BuildAssetBundles()
        {
            HrBundleViewWindow.OpenBundleViewWin();
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
            Debug.Log("Application.persistentDataPath:" + Application.persistentDataPath);
        }


        [MenuItem(STR_HR_TEST)]
        public static void HrTestButton()
        {
            //HrZipFileUtil.PackFiles("E:\\Workspace\\HrGitHub\\HrGameFramework\\HrUnity\\HrFramework\\Assets\\123.zip", "E:\\Workspace\\HrGitHub\\HrGameFramework\\HrUnity\\HrFramework\\Assets\\AssetBundles");
            Debug.Log(SystemInfo.graphicsShaderLevel);
        }

        [MenuItem(STR_ATLAS_TOOL_WIN)]
        public static void OpenAtlasWin()
        {
            HrAtlasMakerWin.OpenHrAtlasMakerWin();
        }
    }
}
