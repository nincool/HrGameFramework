using Hr.Editor.Hierarchy;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetBundleContentView
    {
        private Vector2 m_vc2AssetBundleContentViewScroll = Vector2.zero;

        /// <summary>
        /// Cached
        /// </summary>
        private HrAssetBundle m_cachedSelectedAssetBundle = null;
        private List<HrFileItem> m_lisSelectedItems = new List<HrFileItem>();

        public HrAssetBundleAssignManager EditorManager
        {
            get;
            private set;
        }

        public HrAssetBundleContentView(HrAssetBundleAssignManager editorManager)
        {
            EditorManager = editorManager;
        }

        public bool Init()
        {
            return true;
        }

        public void OnDrawAssetBundleContent()
        {
            m_vc2AssetBundleContentViewScroll = EditorGUILayout.BeginScrollView(m_vc2AssetBundleContentViewScroll);
            {
                if (EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle != null)
                {
                    HrAssetBundle selectedAssetBundle = EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle;
                    var fileItemsArr = selectedAssetBundle.GetAssets();

                    int nRowIndex = 0;
                    foreach (var fileItem in fileItemsArr)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            bool bSelected = fileItem.Selected;
                            if (bSelected != EditorGUILayout.Toggle(bSelected, GUILayout.Width(18f)))
                            {
                                fileItem.Selected = !bSelected;
                            }
                            GUI.DrawTexture(new Rect(24f, 18f * nRowIndex, 18f, 18f), fileItem.Icon);
                            EditorGUILayout.LabelField(string.Empty, GUILayout.Width(18f));
                            EditorGUILayout.LabelField(fileItem.Path);

                            ++nRowIndex;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        public void OnDrawAssetBundleContentMenu()
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        HrAssetBundle selectedAssetBundle = EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle;
                        if (selectedAssetBundle != null &&m_cachedSelectedAssetBundle != selectedAssetBundle)
                        {
                            m_lisSelectedItems.Clear();
                            var fileItemArr = selectedAssetBundle.GetAssets();
                            foreach (var fileItem in fileItemArr)
                            {
                                if (fileItem.Selected)
                                {
                                    m_lisSelectedItems.Add(fileItem);
                                }
                            }
                        }

                        EditorGUI.BeginDisabledGroup(selectedAssetBundle == null || m_lisSelectedItems.Count <= 0);
                        {
                            if (GUILayout.Button("Unassign", GUILayout.Width(140), GUILayout.Height(30)))
                            {
                                foreach (var fileItem in m_lisSelectedItems)
                                {
                                    EditorManager.AssetBundleListManager.UnassignFile(fileItem.AssetBundle.Name, fileItem.AssetBundle.Variant, fileItem);
                                }
                                m_lisSelectedItems.Clear();
                            }
                        }
                        EditorGUI.EndDisabledGroup();

                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
    }
}
