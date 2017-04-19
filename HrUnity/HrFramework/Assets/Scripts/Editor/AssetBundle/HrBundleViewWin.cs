using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace Hr.Editor
{
    public class HrBundleViewWindow : EditorWindow
    {
        static HrBundleViewWindow sWindowInstance;


        private Object[] m_Objs;


        private List<Object> ObjList = new List<Object>();

        //资源

        private Vector2 vector = new Vector2();
        Object mainasset = null;

        public static void OpenBundleViewWin()
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Error", "a scene is running! please stop and try again!", "OK");
                return;
            }

            if (sWindowInstance == null)
            {
                sWindowInstance = GetWindow<HrBundleViewWindow>();
                sWindowInstance.titleContent.text = "PreviewAssetBundle";
                sWindowInstance.titleContent.tooltip = "HrTools";
                sWindowInstance.position = new Rect(200, 200, 1000, 500);
            }

            sWindowInstance.Show();
        }

        void OnGUI()
        {
            if (GUILayout.Button("Select AssetBundle", GUILayout.MaxWidth(180)))
            {
                ClearObj();

                mainasset = null;
                string path = EditorUtility.OpenFilePanel("", Directory.GetParent(Application.dataPath).FullName + "/res", "");
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }
                AssetBundle bundle = AssetBundle.LoadFromFile(path);

                if (bundle != null)
                {
                    mainasset = bundle.mainAsset;

                    Object[] objs = bundle.LoadAllAssets();
                    m_Objs = objs;

                    foreach (Object o in m_Objs)
                    {
                        if (o.GetType() == typeof(GameObject))
                        {
                            Object obj = Instantiate(o);
                            ObjList.Add(obj);
                        }
                    }
                    bundle.Unload(false);
                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("资源实例");
            EditorGUILayout.LabelField("资源类型和名字");
            EditorGUILayout.LabelField("资源大小");
            EditorGUILayout.EndHorizontal();

            vector = EditorGUILayout.BeginScrollView(vector, true, true);
            if (m_Objs != null)
            {
                int i = 0;
                foreach (Object o in m_Objs)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (o == mainasset)
                    {
                        EditorGUILayout.LabelField("Main Asset", GUILayout.MaxWidth(150));
                    }

                    Debug.Log(o.GetType());


                    if (o.GetType() == typeof(GameObject) && ObjList.Count > 0)
                    {
                        if (ObjList[i] != null)
                        {
                            EditorGUILayout.ObjectField(ObjList[i], typeof(GameObject), true);
                        }
                        i++;
                    }

                    //if (o.GetType() == System.Type.GetType("UnityEngine.Texture2D"))
                    //{
                    //    Object obj = Instantiate(o);
                    //    EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
                    //}

                    EditorGUILayout.LabelField(o.GetType() + " : " + o.name);
                    EditorGUILayout.LabelField(UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(o) / 1024f + " KB");
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space();
        }

        private void OnDestroy()
        {
            ClearObj();
        }

        private void ClearObj()
        {
            foreach (var v in ObjList)
            {
                Object.DestroyImmediate(v);
            }

            ObjList.Clear();
        }
    }
}