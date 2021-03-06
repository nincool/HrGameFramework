﻿using Hr.ReleasePool;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetBundleListView
    {
        /// <summary>
        /// 要创建的AssetBundle的名称
        /// </summary>
        private string m_strInputAssetBundleName = string.Empty;
        private string m_strInputAssetBundleVariant = string.Empty;

        public HrAssetBundleAssignManager EditorManager
        {
            get;
            private set;
        }

        public HrAssetBundleListView(HrAssetBundleAssignManager editorManager)
        {
            EditorManager = editorManager;
        }

        public bool Init()
        {

            EditorManager.AssetBundleListManager.FileHierarchy.m_delegateOnToggleAssetBundle += OnToggleAssetBundle;

            return true;
        }

        public void OnToggleAssetBundle(HrAssetBundle assetBundle, bool bSelected)
        {
            if (assetBundle != null && bSelected)
            {
                m_strInputAssetBundleName = assetBundle.Name;
                m_strInputAssetBundleVariant = assetBundle.Variant;
            }
            else
            {
                m_strInputAssetBundleName = string.Empty;
                m_strInputAssetBundleVariant = string.Empty;
            }
        }

        public void OnDrawAssetBundleList()
        {
            EditorManager.AssetBundleListManager.FileHierarchy.DrawHierarchy();
        }

        public void OnDrawAssetBundleListMenu()
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("name:", GUILayout.Width(50f));
                        m_strInputAssetBundleName = EditorGUILayout.TextField(m_strInputAssetBundleName, GUILayout.Width(130f));
                        EditorGUILayout.LabelField("variant:", GUILayout.Width(50f));
                        m_strInputAssetBundleVariant = EditorGUILayout.TextField(m_strInputAssetBundleVariant, GUILayout.Width(100f));

                        if (GUILayout.Button("Add AssetBundle"))
                        {
                            CancelFocus();

                            if (string.IsNullOrEmpty(m_strInputAssetBundleName))
                            {
                                EditorUtility.DisplayDialog("Warning", "AssetBunle name should not be empty!", "OK");
                            }
                            else
                            {
                                EditorUtility.DisplayProgressBar("Add AssetBundle", "Processing...", 0f);

                                EditorManager.AssetBundleListManager.AddAssetBundle(m_strInputAssetBundleName, m_strInputAssetBundleVariant);
                                var assetBundle = EditorManager.AssetBundleListManager.GetAssetBundle(m_strInputAssetBundleName, m_strInputAssetBundleVariant);

                                m_strInputAssetBundleName = string.Empty;
                                m_strInputAssetBundleVariant = string.Empty;

                                EditorUtility.ClearProgressBar();
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(m_strInputAssetBundleName) || EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle == null);
                        {
                            if (GUILayout.Button("Rename", GUILayout.Width(130f), GUILayout.Height(30f)))
                            {
                                EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle.Rename(m_strInputAssetBundleName, m_strInputAssetBundleVariant);
                            }
                        }
                        EditorGUI.EndDisabledGroup();

                        EditorGUI.BeginDisabledGroup(EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle == null);
                        {
                            if (GUILayout.Button("Remove", GUILayout.Width(130f), GUILayout.Height(30f)))
                            {
                                EditorUtility.DisplayProgressBar("Removing", "Remove the assetbundle... please wait..", 0f);
                                {
                                    EditorManager.AssetBundleListManager.RemoveAssetBundle(EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle.Name
                                        , EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle.Variant);
                                }
                                EditorUtility.ClearProgressBar();
                            }
                        }
                        EditorGUI.EndDisabledGroup();

                        EditorGUI.BeginDisabledGroup(EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle == null);
                        {
                            if (EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle != null)
                            {
                                EnumReleaseStrategy releaseStragety = (EnumReleaseStrategy)EditorGUILayout.EnumPopup(EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle.ReleaseStrategy);
                                if (releaseStragety != EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle.ReleaseStrategy)
                                {
                                    EditorManager.AssetBundleListManager.FileHierarchy.CurSelectedAssetBundle.ReleaseStrategy = releaseStragety;
                                }
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

        private void CancelFocus()
        {
            GUI.FocusControl(null);
        }
    }
}

//public enum EnumReleaseStrategy
//{
//    RELEASE_WAITFORENDOFFRAME,  //下一帧释放
//    RELEASE_WAITFORSECONDS,     //在等待一定时间释放
//    RELEASE_SWICHSCENE,         //切换场景释放
//    RELEASE_RESIDENT,           //常驻内存，在APP退出释放
//}