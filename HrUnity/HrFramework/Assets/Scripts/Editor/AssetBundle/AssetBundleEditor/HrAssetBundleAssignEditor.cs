using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hr.Editor
{
    public class HrAssetBundleAssignEditor : EditorWindow
    {
        private HrAssetBundleAssignManager m_assetBundleAssignManager = null;

        /// <summary>
        /// 
        /// </summary>
        private HrAssetBundleListView m_assetBundleListView = null;

        /// <summary>
        /// 
        /// </summary>
        private HrAssetBundleContentView m_assetBundleContentView = null;
        /// <summary>
        /// 
        /// </summary>
        private HrAssetBundleSourceListView m_assetBundleSourceListView = null;
        

        [MenuItem("Hr Tools/AssetBundle Tools/AssetBundle Editor", false, 103)]
        private static void Open()
        {
            HrAssetBundleAssignEditor window = GetWindow<HrAssetBundleAssignEditor>(true, "AssetBundle Editor", true);
            window.minSize = window.maxSize = new Vector2(1400f, 900f);
        }

        private void OnEnable()
        {
            m_assetBundleAssignManager = new HrAssetBundleAssignManager();
            m_assetBundleListView = new HrAssetBundleListView(m_assetBundleAssignManager);
            m_assetBundleContentView = new HrAssetBundleContentView(m_assetBundleAssignManager);
            m_assetBundleSourceListView = new HrAssetBundleSourceListView(m_assetBundleAssignManager);

            m_assetBundleAssignManager.Load();

            m_assetBundleAssignManager.Init();
            m_assetBundleListView.Init();
            m_assetBundleContentView.Init();
            m_assetBundleSourceListView.Init();

        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                EditorGUILayout.HelpBox("Waiting to compile", MessageType.Warning);
                return;
            }
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width), GUILayout.Height(position.height * 0.9f)))
            {
                float fWidthPercent = 0.33f;
                using (new EditorGUILayout.HorizontalScope())
                {
                    #region 1
                    using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * fWidthPercent)))
                    {
                        using (new EditorGUILayout.HorizontalScope(GUILayout.Width(position.width * fWidthPercent), GUILayout.Height(position.height * 0.6f)))
                        {
                            GUILayout.Space(2f);
                            using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * fWidthPercent)))
                            {
                                GUILayout.Space(5f);
                                EditorGUILayout.LabelField(string.Format("AssetBundle List"), EditorStyles.boldLabel);
                                using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(position.height * 0.6f - 10)))
                                {
                                    m_assetBundleListView.OnDrawAssetBundleList();
                                }
                            }

                        }
                        using (new EditorGUILayout.HorizontalScope(GUILayout.Width(position.width * fWidthPercent), GUILayout.Height(position.height * 0.3f)))
                        {
                            GUILayout.Space(2f);
                            using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * fWidthPercent)))
                            {
                                GUILayout.Space(5f);
                                using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(position.height * 0.3f - 10)))
                                {
                                    m_assetBundleListView.OnDrawAssetBundleListMenu();
                                }
                            }
                        }
                    }
                    #endregion

                    #region 2
                    using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * fWidthPercent)))
                    {
                        using (new EditorGUILayout.HorizontalScope(GUILayout.Width(position.width * fWidthPercent), GUILayout.Height(position.height * 0.6f)))
                        {
                            GUILayout.Space(2f);
                            using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * fWidthPercent)))
                            {
                                GUILayout.Space(5f);
                                EditorGUILayout.LabelField(string.Format("AssetBundle List"), EditorStyles.boldLabel);
                                using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(position.height * 0.6f - 10)))
                                {
                                    m_assetBundleContentView.OnDrawAssetBundleContent();
                                }
                            }
                        }
                        using (new EditorGUILayout.HorizontalScope(GUILayout.Width(position.width * fWidthPercent), GUILayout.Height(position.height * 0.3f)))
                        {
                            GUILayout.Space(2f);
                            using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * fWidthPercent)))
                            {
                                GUILayout.Space(5f);
                                using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(position.height * 0.3f - 10)))
                                {
                                    m_assetBundleContentView.OnDrawAssetBundleContentMenu();
                                }
                            }
                        }
                    }
                    #endregion

                    #region 3
                    using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * fWidthPercent)))
                    {
                        using (new EditorGUILayout.HorizontalScope(GUILayout.Width(position.width * fWidthPercent), GUILayout.Height(position.height * 0.6f)))
                        {
                            GUILayout.Space(2f);
                            using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * fWidthPercent)))
                            {
                                GUILayout.Space(5f);
                                EditorGUILayout.LabelField("Asset List");
                                using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(position.height * 0.6f - 10)))
                                {
                                    m_assetBundleSourceListView.OnDrawAssetBundleSource();
                                }
                            }
                        }
                        using (new EditorGUILayout.HorizontalScope(GUILayout.Width(position.width * fWidthPercent), GUILayout.Height(position.height * 0.3f)))
                        {
                            GUILayout.Space(2f);
                            using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * fWidthPercent)))
                            {
                                GUILayout.Space(5f);
                                using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(position.height * 0.3f - 10)))
                                {
                                    m_assetBundleSourceListView.OnDrawAddAssetBundleMenu();
                                }
                            }
                        }
                    }
                    #endregion
                }
                GUILayout.Space(2f);
                using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(position.height * 0.08f)))
                {
                    if (GUILayout.Button("Save", GUILayout.Height(40f)))
                    {
                        EditorUtility.DisplayProgressBar("Save", "Processing...", 0f);

                        m_assetBundleAssignManager.Save();

                        EditorUtility.ClearProgressBar();
                    }
                }
            }

        }
    }

}
