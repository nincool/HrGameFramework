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


        public static bool sbLZ4Compression = true;
        public static bool sbLZMACompression = false;
        public static bool sbUnCompression = false;

        private static string sStrCurrentBuildPath = "";

        private static string sStrOutputPath = "";

        /// <summary>
        /// 设置资源的AssetBundle打包名称
        /// </summary> 
        private static string ms_strAssetName = "";
        private static string ms_strAssetVariant = "";

        private static List<List<string> > slistBuildPath = new List<List<string> >();

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

            Rect rtPlayGround = new Rect(0, 0, position.width, 500);
            Rect rtCompression = new Rect(0, 0, 350, 70);

            GUILayout.BeginArea(rtPlayGround);

            OnGUICheckCompression();

            OnGUIBuildButton();

            OnGUISetAssetBundleName();

            GUILayout.Space(20);

            OnGUIBuildPath();

            GUILayout.EndArea();
        }

        private void OnGUICheckCompression()
        {
            #region CHECK_COMPRESSION
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                HrBuildAssetBundleWin.sbLZ4Compression = EditorGUILayout.ToggleLeft("LZ4 Compression", HrBuildAssetBundleWin.sbLZ4Compression);
                HrBuildAssetBundleWin.sbLZMACompression = EditorGUILayout.ToggleLeft("LZMA Compression", HrBuildAssetBundleWin.sbLZMACompression);
                HrBuildAssetBundleWin.sbUnCompression = EditorGUILayout.ToggleLeft("UnCompression", HrBuildAssetBundleWin.sbUnCompression);

                EditorGUILayout.HelpBox("you must select one mode!", MessageType.Warning);

                EditorGUILayout.EndVertical();
            }
            #endregion

        }

        private void OnGUIBuildButton()
        {
            #region BUILD_BUTTON
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("BuildAssetBundle", EditorStyles.miniButton, GUILayout.Width(120), GUILayout.Height(30)))
                {

                    int nTempCheckCount = 0;
                    if (HrBuildAssetBundleWin.sbLZ4Compression) ++nTempCheckCount;
                    if (HrBuildAssetBundleWin.sbLZMACompression) ++nTempCheckCount;
                    if (HrBuildAssetBundleWin.sbUnCompression) ++nTempCheckCount;
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
                    HrBuildAssetBundle.BuildAssetBundles(lisAssetBundleBuilds.ToArray(), sStrOutputPath);
                }

                if (GUILayout.Button("BuildAll", EditorStyles.miniButton, GUILayout.Width(120), GUILayout.Height(30)))
                {

                }


                EditorGUILayout.EndHorizontal();
            }
            #endregion

        }

        private void OnGUISetAssetBundleName()
        {
            using (new GUILayout.HorizontalScope())
            {
                var rtTextField = EditorGUILayout.GetControlRect(GUILayout.Width(300));
                ms_strAssetName = EditorGUI.TextField(rtTextField, "AssetName:", ms_strAssetName);

                rtTextField.x += 350;
                ms_strAssetVariant = EditorGUI.TextField(rtTextField, "Variant:", ms_strAssetVariant);
            }
            using (new GUILayout.VerticalScope())
            {
                if (GUILayout.Button("SetNameFromSelection", EditorStyles.miniButton, GUILayout.Width(150), GUILayout.Height(30)))
                {
                    if (!string.IsNullOrEmpty(ms_strAssetName) && !string.IsNullOrEmpty(ms_strAssetVariant))
                    {
                        var assetsArr = Selection.objects.Where(o => !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o))).ToArray();

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

                            string assetBundleName = ms_strAssetName.ToLower();
                            string assetBundleVariant = ms_strAssetVariant.ToLower();
                            assetImporter.SetAssetBundleNameAndVariant(assetBundleName, assetBundleVariant);
                            AssetDatabase.Refresh();
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "you must input the name!", "OK");
                        return;
                    }
                }
            }
        }

        private void OnGUIBuildPath()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginVertical();

            for (var i = 0; i < slistBuildPath.Count; ++i)
            {
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("-", GUILayout.Width(30)))
                    {
                        slistBuildPath.RemoveAt(i);
                        break;
                    }
                    else
                    {
                        var rtTextField = EditorGUILayout.GetControlRect(GUILayout.Width(position.width));
                        slistBuildPath[i][0] = EditorGUI.TextField(rtTextField, "Path:", slistBuildPath[i][0]);
                        if ((Event.current.type == EventType.DragUpdated
                            || Event.current.type == EventType.DragExited)
                              && rtTextField.Contains(Event.current.mousePosition))
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                            if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                            {
                                slistBuildPath[i][0] = DragAndDrop.paths[0];
                            }
                        }

                    }
                }
                using (new GUILayout.VerticalScope())
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        var rtTextField = EditorGUILayout.GetControlRect(GUILayout.Width(300));
                        slistBuildPath[i][1] = EditorGUI.TextField(rtTextField, "AssetName:", slistBuildPath[i][1]);

                        rtTextField.x += 350;
                        slistBuildPath[i][2] = EditorGUI.TextField(rtTextField, "Variant:", slistBuildPath[i][2]);

                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        string strPath = HrFileUtil.GetPathWithProjectPath(slistBuildPath[i][0]);
                        if (GUILayout.Button("Select in Window", GUILayout.Width(120)))
                        {
                            var obj = AssetDatabase.LoadMainAssetAtPath(slistBuildPath[i][0]);
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
                                string strAssetsDir = item.Substring(item.IndexOf("Assets"));
                                var assetImporter = AssetImporter.GetAtPath(strAssetsDir);

                                if (assetImporter == null)
                                {
                                    HrLoger.LogError("asset importer is null! path:" + item);
                                    continue;
                                }

                                // Get asset bundle name & variant
                                var assetBundleName = assetImporter.assetBundleName;
                                var assetBundleVariant = assetImporter.assetBundleVariant;

                                if (!string.IsNullOrEmpty(slistBuildPath[i][1]) )
                                {
                                    assetBundleName = slistBuildPath[i][1].ToLower();
                                }
                                if (!string.IsNullOrEmpty(slistBuildPath[i][2]))
                                {
                                    assetBundleVariant = slistBuildPath[i][2].ToLower();
                                }
                                assetImporter.SetAssetBundleNameAndVariant(assetBundleName, assetBundleVariant);
                                AssetDatabase.Refresh();

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
                            HrBuildAssetBundle.BuildAssetBundles(lisAssetBundleBuilds.ToArray(), sStrOutputPath);
                        }
                    }
                }
            }

            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Add One Item"))
            {
                var lis = new List<string>();
                lis.Add("");
                lis.Add("");
                lis.Add("");
                slistBuildPath.Add(lis);
            }
               
            EditorGUILayout.EndVertical();

            GUILayout.Space(20);

            using (new GUILayout.VerticalScope())
            {
                var rtTextField = EditorGUILayout.GetControlRect(GUILayout.Width(position.width));

                if (string.IsNullOrEmpty(sStrOutputPath))
                {
                    sStrOutputPath = EditorGUI.TextField(rtTextField, "OutputPath:", HrBuildAssetBundle.CreateAssetBundleDirectory());
                }
                else
                {
                    sStrOutputPath = EditorGUI.TextField(rtTextField, "OutputPath:", sStrOutputPath);
                }

                string strPath = HrFileUtil.GetPathWithProjectPath(sStrOutputPath);
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

                    if (GUILayout.Button("Show in Explorer", GUILayout.Width(120)))
                    {

                        if (!Directory.Exists(strPath))
                            Directory.CreateDirectory(strPath);
                        EditorUtility.RevealInFinder(strPath);
                    }

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
            }
        }

        private void OnInspectorGUI()
        {
            
        }


    }
}

