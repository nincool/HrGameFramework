using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Hr.Editor
{
    public class HrAssetBundleSelectedBuilder : EditorWindow
    {
        [MenuItem("Hr Tools/AssetBundle Tools/AssetBundle Selected Builder", false, 103)]
        private static void Opne()
        {
            HrAssetBundleSelectedBuilder window = GetWindow<HrAssetBundleSelectedBuilder>(true, "AssetBundle Builder", true);
            window.minSize = window.maxSize = new Vector2(800f, 700f);
        }

        private void OnEnable()
        {

        }

        private void OnGUI()
        {
            if (GUILayout.Button("BuildFromSelection", EditorStyles.miniButton, GUILayout.Width(120), GUILayout.Height(30)))
            {
                var assetsArr = Selection.objects.Where(o => !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o))).ToArray();

                List<AssetBundleBuild> lisAssetBundleBuilds = new List<AssetBundleBuild>();
                HashSet<string> processedBundles = new HashSet<string>();

                List<string> lisAssetPath = new List<string>();
                // Get asset bundle names from selection
                foreach (var o in assetsArr)
                {
                    var strAssetPath = AssetDatabase.GetAssetPath(o);
                    var assetImporter = AssetImporter.GetAtPath(strAssetPath);

                    if (assetImporter == null)
                    {
                        HrLogger.LogError("asset importer is null! path:" + strAssetPath);
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

                    foreach (var strAssetName in build.assetNames)
                    {
                        lisAssetPath.Add(strAssetName);
                    }

                    lisAssetBundleBuilds.Add(build);
                }

                const string strOutputPath = "E:/Workspace/HrGitHub/HrGameFramework/HrUnity/AssetBundles/SingleSelected";

                HrBuildAssetBundle.BuildAssetBundles(lisAssetBundleBuilds.ToArray(), strOutputPath);
                //HrJsonUtil.SaveJsonFile(Application.dataPath + "/" + HrResourcePath.STR_ASSETBUNDLES_OUTPUT_PATH)
            }

        }
    }

}
