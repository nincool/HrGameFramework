using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
using Hr.CommonUtility;

namespace Hr.EditorAssetBundle
{
    public class HrBuildAssetBundleWin : EditorWindow
    {
        static HrBuildAssetBundleWin sWindowInstance;

        private static string sStrCurrentBuildPath = "";

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
                sWindowInstance.position = new Rect(200, 200, 1000, 500);
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

            #region CHECK_COMPRESSION
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                HrAssetBundleManager.sbLZ4Compression = EditorGUILayout.ToggleLeft("LZ4 Compression", HrAssetBundleManager.sbLZ4Compression);
                HrAssetBundleManager.sbLZMACompression = EditorGUILayout.ToggleLeft("LZMA Compression", HrAssetBundleManager.sbLZMACompression);
                HrAssetBundleManager.sbUnCompression = EditorGUILayout.ToggleLeft("UnCompression", HrAssetBundleManager.sbUnCompression);

                EditorGUILayout.HelpBox("you must select one mode!", MessageType.Warning);

                EditorGUILayout.EndVertical();
            }
            #endregion

            #region BUILD_BUTTON
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("BuildAssetBundle", EditorStyles.miniButton, GUILayout.Width(120), GUILayout.Height(30)))
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

                if (GUILayout.Button("BuildFromSelection", EditorStyles.miniButton, GUILayout.Width(120), GUILayout.Height(30)))
                {
                    var assetsArr = Selection.objects.Where(o => !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o))).ToArray();

                    List<AssetBundleBuild> lisAssetBundleBuilds = new List<AssetBundleBuild>();
                    HashSet<string> processedBundles = new HashSet<string>();

                    // Get asset bundle names from selection
                    foreach (var o in assetsArr)
                    {
                        var strAssetPath = AssetDatabase.GetAssetPath(o);
                        var assetImporter = AssetImporter.GetAtPath(strAssetPath);

                        if (assetImporter == null)
                        {
                            HrLoger.LogError("asset importer is null! path:" + strAssetPath);
                            continue;
                        }

                        // Get asset bundle name & variant
                        var assetBundleName = assetImporter.assetBundleName;
                        var assetBundleVariant = assetImporter.assetBundleVariant;
                        var assetBundleFullName = string.IsNullOrEmpty(assetBundleVariant) ? assetBundleName : assetBundleName + "." + assetBundleVariant;

                        // Only process assetBundleFullName once. No need to add it again.
                        if (processedBundles.Contains(assetBundleFullName))
                        {
                            continue;
                        }

                        processedBundles.Add(assetBundleFullName);

                        AssetBundleBuild build = new AssetBundleBuild();

                        build.assetBundleName = assetBundleName;
                        build.assetBundleVariant = assetBundleVariant;
                        build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleFullName);

                        lisAssetBundleBuilds.Add(build);
                    }
                    HrBuildAssetBundle.BuildAssetBundles(lisAssetBundleBuilds.ToArray());
                }

                if (GUILayout.Button("BuildAll", EditorStyles.miniButton, GUILayout.Width(120), GUILayout.Height(30)))
                {

                }


                EditorGUILayout.EndHorizontal();
            }
            #endregion

            #region BUILD_PATH
            {
                EditorGUILayout.BeginVertical();

                string strRootPath = "Assets";
                EditorGUILayout.LabelField("RootPath:" + strRootPath, GUILayout.Width(280));

                var rtTextField = EditorGUILayout.GetControlRect(GUILayout.Width(position.width));
                var strTempPath = EditorGUI.TextField(rtTextField, "", sStrCurrentBuildPath);
                if (strTempPath != sStrCurrentBuildPath)
                {
                    sStrCurrentBuildPath = strTempPath;
                }
                if ((Event.current.type == EventType.DragUpdated
                  || Event.current.type == EventType.DragExited)
                  && rtTextField.Contains(Event.current.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                    if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                    {
                        sStrCurrentBuildPath = DragAndDrop.paths[0];
                    }
                }
                EditorGUILayout.BeginHorizontal();

                string strPath = HrFileUtil.GetPathWithProjectPath(sStrCurrentBuildPath);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Select in Window", GUILayout.Width(120)))
                {
                    var obj = AssetDatabase.LoadMainAssetAtPath(sStrCurrentBuildPath);
                    EditorGUIUtility.PingObject(obj);
                }
                if (GUILayout.Button("Show in Explorer", GUILayout.Width(120)))
                {
                   
                    if (!Directory.Exists(strPath))
                        Directory.CreateDirectory(strPath);
                    EditorUtility.RevealInFinder(strPath);
                }
                if (GUILayout.Button("Build Path", GUILayout.Width(120)))
                {
                    var strFilePathArr = HrFileUtil.GetAllFilePathsInFolder(strPath);
                    List<AssetBundleBuild> lisAssetBundleBuilds = new List<AssetBundleBuild>();
                    HashSet<string> processedBundles = new HashSet<string>();

                    foreach (var item in strFilePathArr)
                    {
                        var assetImporter = AssetImporter.GetAtPath(item);

                        if (assetImporter == null)
                        {
                            HrLoger.LogError("asset importer is null! path:" + item);
                            continue;
                        }

                        // Get asset bundle name & variant
                        var assetBundleName = assetImporter.assetBundleName;
                        var assetBundleVariant = assetImporter.assetBundleVariant;
                        var assetBundleFullName = string.IsNullOrEmpty(assetBundleVariant) ? assetBundleName : assetBundleName + "." + assetBundleVariant;

                        // Only process assetBundleFullName once. No need to add it again.
                        if (processedBundles.Contains(assetBundleFullName))
                        {
                            continue;
                        }

                        processedBundles.Add(assetBundleFullName);

                        AssetBundleBuild build = new AssetBundleBuild();

                        build.assetBundleName = assetBundleName;
                        build.assetBundleVariant = assetBundleVariant;
                        build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleFullName);

                        lisAssetBundleBuilds.Add(build);
                    }
                    HrBuildAssetBundle.BuildAssetBundles(lisAssetBundleBuilds.ToArray());
                }

                EditorGUILayout.EndHorizontal();

                if (!Directory.Exists(strPath))
                {
                    EditorGUILayout.LabelField("Available Directories:");
                    string strDirectName = Path.GetDirectoryName(strPath);
                    if (Directory.Exists(strDirectName))
                    {
                        string[] dirs = Directory.GetDirectories(strDirectName);
                        foreach (string s in dirs)
                        {
                            EditorGUILayout.LabelField(s);
                        }
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("Exist Assets:");
                    //except asset folder
                    if (Path.GetDirectoryName(strPath).Contains("Assets") 
                        && Event.current.type != EventType.DragUpdated
                        && Event.current.type != EventType.DragExited)
                    {
                        var strFilePathArr = HrFileUtil.GetAllFilePathsInFolder(strPath);
                        foreach (var item in strFilePathArr)
                        {
                            GUILayout.Label(item, GUILayout.Width(position.width));
                            //EditorGUILayout.LabelField(item);
                        }
                    }
                }

                EditorGUILayout.EndVertical();
            }

            #endregion

            GUILayout.EndArea();
        }

        private void OnInspectorGUI()
        {
            
        }


    }
}

