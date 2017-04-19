using Hr;
using Hr;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public class HrZipFileWin : EditorWindow
    {
        static HrZipFileWin sWindowInstance;

        private static string sFromPath;
        private static string sToPath;

        public static void OpenZipFileWin()
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error", "a scene is running! please stop and try again!", "OK");
                return;
            }

            if (sWindowInstance == null)
            {
                sWindowInstance = GetWindow<HrZipFileWin>();
                sWindowInstance.titleContent.text = "ZipFile";
                sWindowInstance.titleContent.tooltip = "HrTools";
                sWindowInstance.position = new Rect(200, 200, 1000, 500);
            }
            if (string.IsNullOrEmpty(sFromPath))
            {
                sFromPath = HrResourcePath.ms_strAssetBundleOutputPath;
            }
            if (string.IsNullOrEmpty(sToPath))
            {
                sToPath = HrResourcePath.ms_strZipAssetBundlePath;
            }

            sWindowInstance.Show();
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

            using (new GUILayout.VerticalScope())
            {
                var rtTextField = EditorGUILayout.GetControlRect(GUILayout.Width(position.width));
                sFromPath = EditorGUI.TextField(rtTextField, "SrcPath:", sFromPath);
                rtTextField = EditorGUILayout.GetControlRect(GUILayout.Width(position.width));
                sToPath = EditorGUI.TextField(rtTextField, "DstPath", sToPath);

                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Show in Explorer", GUILayout.Width(120)))
                    {
                        EditorUtility.RevealInFinder(sToPath);
                    }
                    if (GUILayout.Button("Compress", GUILayout.Width(120)))
                    {
                        HrZipFileUtil.PackFiles(sToPath, sFromPath);
                    }
                }
            }

            GUILayout.EndArea();
        }

    }

}
