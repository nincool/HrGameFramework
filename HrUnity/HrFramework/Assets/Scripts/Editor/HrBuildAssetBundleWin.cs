using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using HrCommonUtility;

namespace HrEditorAssetBundle
{
    public class HrBuildAssetBundleWin : EditorWindow
    {
        static HrBuildAssetBundleWin sWindowInstance;

        public static void OpenBuildAssetBundleWin()
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error", "a scene is running! please stop and try again!", "OK");
                return;
            }

            if (sWindowInstance == null)
            {
                sWindowInstance = GetWindow<HrBuildAssetBundleWin>();
                sWindowInstance.titleContent.text = "BuildAssetBundle";
                sWindowInstance.titleContent.tooltip = "HrTools";
            }

            sWindowInstance.Show();
        }

        private void Awake()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                EditorGUILayout.HelpBox("Waiting to compile", MessageType.Warning);
                return;
            }

            Rect rtToggleCompression = new Rect(0, 0, position.width, 500);

            GUILayout.BeginArea(rtToggleCompression);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            HrAssetBundleManager.sbLZ4Compression = EditorGUILayout.ToggleLeft("LZ4 Compression", HrAssetBundleManager.sbLZ4Compression);
            HrAssetBundleManager.sbLZMACompression = EditorGUILayout.ToggleLeft("LZMA Compression", HrAssetBundleManager.sbLZMACompression);
            HrAssetBundleManager.sbUnCompression = EditorGUILayout.ToggleLeft("UnCompression", HrAssetBundleManager.sbUnCompression);

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("BuildAssetBundle", EditorStyles.miniButton, GUILayout.Width(100), GUILayout.Height(30)))
            {
                
                int nTempCheckCount = 0;
                if (HrAssetBundleManager.sbLZ4Compression) ++nTempCheckCount;
                if (HrAssetBundleManager.sbLZMACompression) ++nTempCheckCount;
                if (HrAssetBundleManager.sbUnCompression) ++nTempCheckCount;
                if (nTempCheckCount != 1)
                {
                    EditorUtility.DisplayDialog("Error", "you must select one mode!", "OK");
                    return;
                }
                else
                {
                    bool bCanPressed = true;
                    bool shouldCheckODR = EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS;
#if UNITY_TVOS
                    shouldCheckODR |= EditorUserBuildSettings.activeBuildTarget == BuildTarget.tvOS;
#endif
                    if (shouldCheckODR)
                    {
#if ENABLE_IOS_ON_DEMAND_RESOURCES
                        if (PlayerSettings.iOS.useOnDemandResources)
                            bCanPressed = false;
#endif
#if ENABLE_IOS_APP_SLICING
                            bCanPressed = false;;
#endif
                    }
                    if (!bCanPressed)
                    {
                        EditorUtility.DisplayDialog("Error", "Can not compress!", "OK");
                        return;
                    }
                    HrBuildAssetBundle.BuildAssetBundles();
                }
            }
            EditorGUILayout.HelpBox("you must select one mode!", MessageType.Warning);
            EditorGUILayout.EndVertical();


            GUILayout.EndArea();
        }

        private void OnInspectorGUI()
        {
            
        }


    }
}

