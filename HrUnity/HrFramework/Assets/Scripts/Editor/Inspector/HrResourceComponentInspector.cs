using Hr.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Hr.Editor
{
    [CustomEditor(typeof(HrResourceComponent))]
    public class HrResourceComponentInspector : HrInspector
    {
        private List<bool> m_lisAssetFileSelected = new List<bool>();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            HrResourceComponent inspectTarget = (HrResourceComponent)target;

            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.LabelField(string.Format("Init Success:{0}", inspectTarget.InitSuccess));
            }

            GUILayout.Space(20f);

            List<HrAssetFile> lisAllAssetFiles = inspectTarget.GetAllAssetFiles();
            if (m_lisAssetFileSelected.Count < lisAllAssetFiles.Count)
            {
                for (var i = 0; i < 100; ++i)
                {
                    m_lisAssetFileSelected.Add(false);
                }
            }
                
            EditorGUILayout.LabelField(string.Format("AssetBundles:{0}", lisAllAssetFiles.Count), EditorStyles.boldLabel);
            using (new GUILayout.VerticalScope("box"))
            {
                //foreach (var iteAssetFile in lisAllAssetFiles)
                for (var i = 0; i < lisAllAssetFiles.Count; ++i)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Space(10f);
                        bool bExpand = EditorGUILayout.Foldout(m_lisAssetFileSelected[i], lisAllAssetFiles[i].Name);
                        if (bExpand != m_lisAssetFileSelected[i])
                        {
                            m_lisAssetFileSelected[i] = bExpand;
                        }
                    }

                    if (m_lisAssetFileSelected[i])
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            EditorGUILayout.LabelField(string.Format("  FullPath:{0}", lisAllAssetFiles[i].FullPath));
                            EditorGUILayout.LabelField(string.Format("  Type:{0}", lisAllAssetFiles[i].GetType()));
                            EditorGUILayout.LabelField(string.Format("  ReleaseStrategy:{0}", lisAllAssetFiles[i].ReleaseStrategy.ReleaseStrategy));
                            EditorGUILayout.LabelField(string.Format("  Status:{0}", lisAllAssetFiles[i].AssetBundleStatus));
                            EditorGUILayout.LabelField(string.Format("  IsError:{0}", lisAllAssetFiles[i].IsError()));
                        }
                    }
                }
            }
        }

        private void UnselectedAllAssetBundle()
        {
            for (var i = 0; i < m_lisAssetFileSelected.Count; ++i)
            {
                m_lisAssetFileSelected[i] = false;
            }
        }
    }
}


