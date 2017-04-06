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
    public class HrSerializeDataWin : EditorWindow
    {
        static HrSerializeDataWin sWindowInstance;


        public static void OpenSerializeDataWin()
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error", "a scene is running! please stop and try again!", "OK");
                return;
            }

            if (sWindowInstance == null)
            {
                sWindowInstance = GetWindow<HrSerializeDataWin>();
                sWindowInstance.titleContent.text = "SerializeData";
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

            #region BUILD_BUTTON
            {
                EditorGUILayout.BeginHorizontal();


                if (GUILayout.Button("BuildFromSelection", EditorStyles.miniButton, GUILayout.Width(120), GUILayout.Height(30)))
                {
                    var testData = ScriptableObject.CreateInstance<HrSampleData>();
                    testData.mStrData = "ssssaaaassdd";
                    testData.mLisData.Add(1);
                    testData.mLisData.Add(2);
                    testData.mIntData = 99;
                    AssetDatabase.CreateAsset(testData, "Assets/ScritableData/test.asset");
                    AssetDatabase.Refresh();
                }

                EditorGUILayout.EndHorizontal();
            }
            #endregion


            GUILayout.EndArea();
        }

        private void OnInspectorGUI()
        {

        }


    }
}

