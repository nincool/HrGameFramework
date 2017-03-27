using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HrEditorAssetBundles
{
    public class hrAssetBundleMenuItems
    {
        const string STR_SIMULATION_MODE = "HrTools/Assets/AssetBundles/Simulation Mode";
        const string STR_BUILD_ASSETBUNDLES = "HrTools/Assets/AssetBundles/Build AssetBundles";

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



    }
}
