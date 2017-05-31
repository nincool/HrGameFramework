using Hr.Editor.Hierarchy;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetBundleSourceListView
    {
        public HrAssetBundleAssignManager EditorManager
        {
            get;
            private set;
        }

        public HrAssetBundleSourceListView(HrAssetBundleAssignManager editorManager)
        {
            EditorManager = editorManager;
        }

        public bool Init()
        {
            return true;
        }

        public void OnDrawAssetBundleSource()
        {
            EditorManager.SourceListManager.FileHierarchy.DrawHierarchy();
        }

        public void OnDrawAddAssetBundleMenu()
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            EditorManager.SourceListManager.FileHierarchy.AssignedFilesVisible = EditorGUILayout.Toggle("AssignedFilesVisible", EditorManager.SourceListManager.FileHierarchy.AssignedFilesVisible);

                            GUILayout.Space(10f);

                            EditorGUILayout.BeginHorizontal();
                            {
                                HashSet<HrFileItem> setSelectedFiles = EditorManager.SourceListManager.GetSelectedFiles();
                                EditorGUI.BeginDisabledGroup(EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle == null || setSelectedFiles.Count <= 0);
                                {
                                    if (GUILayout.Button(string.Format("Add {0} ", setSelectedFiles.Count), GUILayout.Width(130f), GUILayout.Height(30f)))
                                    {
                                        foreach (var subSelectFile in setSelectedFiles)
                                        {
                                            if (!EditorManager.SourceListManager.AssignAsset(subSelectFile, EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle))
                                            {
                                                EditorUtility.DisplayDialog("Warning", string.Format("asset '{0}' is already assigned assetbundle '{1}'", subSelectFile.Name, subSelectFile.AssetBundle.Name), "OK");
                                            }
                                        }
                                    }
                                }
                                EditorGUI.EndDisabledGroup();

                                EditorGUI.BeginDisabledGroup(setSelectedFiles.Count <= 0);
                                {
                                    if (GUILayout.Button(string.Format("Remove {0}", setSelectedFiles.Count), GUILayout.Width(130f), GUILayout.Height(30f)))
                                    {
                                        foreach (var subSelectFile in setSelectedFiles)
                                        {
                                            if (subSelectFile.AssetBundle != null)
                                            {
                                                if (EditorManager.SourceListManager.UnAssignAsset(subSelectFile))
                                                {
                                                    Debug.Log(string.Format("UnassignAsset {0} from assetbundle success! ", subSelectFile.Name));
                                                    EditorManager.SourceListManager.FileHierarchy.UnselectAll();
                                                }
                                                else
                                                {
                                                    Debug.LogWarning(string.Format("UnassignAsset {0} from assetbundle{1} failed! ", subSelectFile.Name, subSelectFile.AssetBundle.FullName));
                                                }
                                            }

                                        }
                                    }
                                }
                                EditorGUI.EndDisabledGroup();
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
    }
}
