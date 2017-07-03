using Hr.Resource;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HrAtlasMakerWin : EditorWindow
{

    private static HrAtlasMakerWin sWindowInstance;
    private string m_strSrcPath = "";
    private string m_strDstPath = "";

    public static void OpenHrAtlasMakerWin()
    {
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Error", "a scene is running! please stop and try again!", "OK");
            return;
        }

        if (sWindowInstance == null)
        {
            sWindowInstance = GetWindow<HrAtlasMakerWin>();
            sWindowInstance.titleContent.text = "AtlasMaker";
            sWindowInstance.titleContent.tooltip = "HrTools";
            sWindowInstance.position = new Rect(200, 200, 1000, 500);
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

        using (new GUILayout.VerticalScope())
        {
            var rtTextField = EditorGUILayout.GetControlRect(GUILayout.Width(position.width));
            m_strSrcPath = EditorGUI.TextField(rtTextField, "SrcPath:", m_strSrcPath);
            if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited) && rtTextField.Contains(Event.current.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                {
                    m_strSrcPath = DragAndDrop.paths[0];
                }
            }
            GUILayout.Space(20);
            rtTextField = EditorGUILayout.GetControlRect(GUILayout.Width(position.width));
            m_strDstPath = EditorGUI.TextField(rtTextField, "DstPath", m_strDstPath);
            if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited) && rtTextField.Contains(Event.current.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                {
                    m_strDstPath = DragAndDrop.paths[0];
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                //if (GUILayout.Button("Make Atlas", GUILayout.Width(120)))
                //{
                //    if (!Directory.Exists(m_strDstPath))
                //    {
                //        Directory.CreateDirectory(m_strDstPath);
                //    }

                //    string strSrcPath = HrResourcePath.ms_strWorkingPath + m_strSrcPath + "/";
                //    if (!Directory.Exists(strSrcPath))
                //    {
                //        EditorGUILayout.HelpBox("FilePath:" + strSrcPath + " is not exists! ", MessageType.Warning);
                //        return;
                //    }

                //    DirectoryInfo rootDirInfo = new DirectoryInfo(HrResourcePath.ms_strWorkingPath + m_strSrcPath);
                //    var pngFileArr = rootDirInfo.GetFiles("*.png", SearchOption.AllDirectories);
                //    foreach (FileInfo pngFile in pngFileArr)
                //    {
                //        string allPath = pngFile.FullName;
                //        string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
                //        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                //        GameObject go = new GameObject(sprite.name);
                //        go.AddComponent<SpriteRenderer>().sprite = sprite;
                //        allPath = m_strDstPath + "/" + sprite.name + ".prefab";
                //        string prefabPath = allPath.Substring(allPath.IndexOf("Assets"));
                //        PrefabUtility.CreatePrefab(prefabPath, go);
                //        GameObject.DestroyImmediate(go);
                //    }
                //}
            }
        }
    }
}
