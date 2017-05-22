//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
// 来自GameFramework 学习框架 眼过千遍不如手过一遍
//------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hr.Editor
{
    /// <summary>
    /// 资源包编辑器。
    /// </summary>
    public sealed class HrAssetBundleEditor : EditorWindow
    {
        private HrAssetBundleEditorController m_editorControler = new HrAssetBundleEditorController();


        private Vector2 m_vc2AssetBundleViewScroll = Vector2.zero;
        private Vector2 m_vc2SourceAssetsViewScroll = Vector2.zero;

        private HrAssetBundleFolder m_assetBundleRoot = new HrAssetBundleFolder("AssetBundles", null);

        /// <summary>
        /// 已经展开的AssetBundle路径名称
        /// </summary>
        private HashSet<string> m_setExpandedAssetBundleFolderNames = new HashSet<string>();

        /// <summary>
        /// 已经展开的Source路径名称
        /// </summary>
        private HashSet<HrSourceFolder> m_setExpandedSourceFolders = new HashSet<HrSourceFolder>();

        /// <summary>
        /// 已经选择的资源
        /// </summary>
        private HashSet<HrSourceAsset> m_setSelectedSourceAssets = new HashSet<HrSourceAsset>();

        private HashSet<HrSourceFolder> m_setCachedSelectedSourceFolders = new HashSet<HrSourceFolder>();
        private HashSet<HrSourceFolder> m_setCachedUnselectedSourceFolders = new HashSet<HrSourceFolder>();
        private HashSet<HrSourceFolder> m_setCachedAssignedSourceFolders = new HashSet<HrSourceFolder>();
        private HashSet<HrSourceFolder> m_setCachedUnassignedSourceFolders = new HashSet<HrSourceFolder>();
        private HashSet<HrSourceAsset> m_setCachedAssignedSourceAssets = new HashSet<HrSourceAsset>();
        private HashSet<HrSourceAsset> m_setCachedUnassignedSourceAssets = new HashSet<HrSourceAsset>();

        /// <summary>
        /// 隐藏已经分配的资源
        /// </summary>
        private bool m_bHideAssignedSourceAssets = false;

        /// <summary>
        /// 要创建的AssetBundle的名称
        /// </summary>
        private string m_strInputAssetBundleName = string.Empty;
        private string m_strInputAssetBundleVariant = string.Empty;

        /// <summary>
        /// 绘制AssetBundleList 绘制到哪一行的标记
        /// </summary>
        private int m_nCurrentAssetBundleListRowOnDraw = 0;

        [MenuItem("Hr Tools/AssetBundle Tools/AssetBundle Editor", false, 102)]
        private static void Open()
        {
            HrAssetBundleEditor window = GetWindow<HrAssetBundleEditor>(true, "AssetBundle Editor", true);
            window.minSize = new Vector2(1400f, 900f);
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                EditorGUILayout.HelpBox("Waiting to compile", MessageType.Warning);
                return;
            }
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
                            //EditorGUILayout.LabelField(string.Format("AssetBundle List({0})", ))
                            using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(position.height * 0.6f - 10)))
                            {
                                DrawAssetBundleView();
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {

                            }
                        }

                    }
                    using (new EditorGUILayout.HorizontalScope(GUILayout.Width(position.width * fWidthPercent), GUILayout.Height(position.height * 0.4f)))
                    {
                        GUILayout.Space(2f);
                        using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * fWidthPercent)))
                        {
                            GUILayout.Space(5f);
                            //EditorGUILayout.LabelField(string.Format("AssetBundle List({0})", ))
                            using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(position.height * 0.4f - 10)))
                            {
                                DrawAssetBundleListMenu();
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {

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
                            //EditorGUILayout.LabelField(string.Format("AssetBundle List({0})", ))
                            using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(position.height * 0.6f - 10)))
                            {
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {

                            }
                        }
                    }
                    using (new EditorGUILayout.HorizontalScope(GUILayout.Width(position.width * fWidthPercent), GUILayout.Height(position.height * 0.4f)))
                    {
                        GUILayout.Space(2f);
                        using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * fWidthPercent)))
                        {
                            GUILayout.Space(5f);
                            //EditorGUILayout.LabelField(string.Format("AssetBundle List({0})", ))
                            using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(position.height * 0.4f - 10)))
                            {
                                DrawAssetBundleListMenu();
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {

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
                                DrawSourceAssetsView();
                            }
                        }
                    }
                    using (new EditorGUILayout.HorizontalScope(GUILayout.Width(position.width * fWidthPercent), GUILayout.Height(position.height * 0.3f)))
                    {
                        GUILayout.Space(2f);
                        using (new EditorGUILayout.VerticalScope(GUILayout.Width(position.width * fWidthPercent)))
                        {
                            GUILayout.Space(5f);
                            //EditorGUILayout.LabelField(string.Format("AssetBundle List({0})", ))
                            using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(position.height * 0.37f)))
                            {
                                DrawAssetBundleListMenu();
                            }
                        }
                    }
                }
                #endregion

            }

        }

        private void DrawSourceAssetsView()
        {
            int nAssetSourceRowOnDraw = 0;
            m_vc2SourceAssetsViewScroll = EditorGUILayout.BeginScrollView(m_vc2SourceAssetsViewScroll);
            {
                DrawSourceFolder(m_editorControler.SourceAssetRoot, ref nAssetSourceRowOnDraw);
            }
            EditorGUILayout.EndScrollView();
        }

        private bool IsSelectedSourceFolder(HrSourceFolder sourceFolder)
        {
            if (m_setCachedSelectedSourceFolders.Contains(sourceFolder))
            {
                return true;
            }

            if (m_setCachedUnselectedSourceFolders.Contains(sourceFolder))
            {
                return false;
            }

            foreach (HrSourceAsset sourceAsset in sourceFolder.GetAssets())
            {
                if (m_bHideAssignedSourceAssets && IsAssignedSourceAsset(sourceAsset))
                {
                    continue;
                }

                if (!IsSelectedSourceAsset(sourceAsset))
                {
                    m_setCachedUnselectedSourceFolders.Add(sourceFolder);
                    return false;
                }
            }

            foreach (HrSourceFolder subSourceFolder in sourceFolder.GetFolders())
            {
                if (m_bHideAssignedSourceAssets && IsAssignedSourceFolder(sourceFolder))
                {
                    continue;
                }

                if (!IsSelectedSourceFolder(subSourceFolder))
                {
                    m_setCachedUnselectedSourceFolders.Add(sourceFolder);
                    return false;
                }
            }

            m_setCachedSelectedSourceFolders.Add(sourceFolder);
            return true;
        }

        private bool IsAssignedSourceAsset(HrSourceAsset sourceAsset)
        {
            if (m_setCachedAssignedSourceAssets.Contains(sourceAsset))
            {
                return true;
            }

            if (m_setCachedUnassignedSourceAssets.Contains(sourceAsset))
            {
                return false;
            }

            return m_editorControler.GetAsset(sourceAsset.Guid) != null;
        }

        private bool IsAssignedSourceFolder(HrSourceFolder sourceFolder)
        {
            if (m_setCachedAssignedSourceFolders.Contains(sourceFolder))
            {
                return true;
            }

            if (m_setCachedUnassignedSourceFolders.Contains(sourceFolder))
            {
                return false;
            }

            foreach (HrSourceAsset sourceAsset in sourceFolder.GetAssets())
            {
                if (!IsAssignedSourceAsset(sourceAsset))
                {
                    m_setCachedUnassignedSourceFolders.Add(sourceFolder);
                    return false;
                }
            }

            foreach (HrSourceFolder subSourceFolder in sourceFolder.GetFolders())
            {
                if (!IsAssignedSourceFolder(subSourceFolder))
                {
                    m_setCachedUnassignedSourceFolders.Add(sourceFolder);
                    return false;
                }
            }

            m_setCachedAssignedSourceFolders.Add(sourceFolder);
            return true;

        }

        private bool IsSelectedSourceAsset(HrSourceAsset sourceAsset)
        {
            return m_setSelectedSourceAssets.Contains(sourceAsset);
        }

        /// <summary>
        /// 获取选中的资源，已经装配的资源不会返回
        /// </summary>
        /// <returns></returns>
        private HashSet<HrSourceAsset> GetSelectedSourceAssets()
        {
            if (!m_bHideAssignedSourceAssets)
            {
                return m_setSelectedSourceAssets;
            }

            HashSet<HrSourceAsset> selectedUnassignedSourceAssets = new HashSet<HrSourceAsset>();
            foreach (HrSourceAsset sourceAsset in m_setSelectedSourceAssets)
            {
                if (!IsAssignedSourceAsset(sourceAsset))
                {
                    selectedUnassignedSourceAssets.Add(sourceAsset);
                }
            }

            return selectedUnassignedSourceAssets;
        }


        private void DrawSourceFolder(HrSourceFolder sourceFolder, ref int nAssetSourceRowOnDraw)
        {
            bool bExpand = IsExpandedSourceFolder(sourceFolder);
            EditorGUILayout.BeginHorizontal();
            {
                bool bSelect = IsSelectedSourceFolder(sourceFolder);
                if (bSelect != EditorGUILayout.Toggle(bSelect, GUILayout.Width(12f + 14f * sourceFolder.Depth)))
                {
                    bSelect = !bSelect;
                    SetSelectedSourceFolder(sourceFolder, bSelect);
                }

                GUILayout.Space(14f * sourceFolder.Depth);
                if (bExpand != EditorGUI.Foldout(new Rect(18f + 14f * sourceFolder.Depth, 20f * nAssetSourceRowOnDraw + 2f, int.MaxValue, 14f), bExpand, string.Empty, true))
                {
                    bExpand = !bExpand;
                    SetExpandedSourceFolder(sourceFolder, bExpand);
                }

                GUI.DrawTexture(new Rect(32f + 14f * sourceFolder.Depth, 20f * nAssetSourceRowOnDraw + 1f, 16f, 16f), HrSourceFolder.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(26f + 14f * sourceFolder.Depth), GUILayout.Height(18f));
                EditorGUILayout.LabelField(sourceFolder.Name);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetExpandedSourceFolder(HrSourceFolder sourceFolder, bool bExpand)
        {
            if (bExpand)
            {
                m_setExpandedSourceFolders.Add(sourceFolder);
            }
            else
            {
                m_setExpandedSourceFolders.Remove(sourceFolder);
            }
        }

        private void SetSelectedSourceAsset(HrSourceAsset sourceAsset, bool bSelect)
        {
            if (bSelect)
            {
                m_setSelectedSourceAssets.Add(sourceAsset);

                HrSourceFolder folder = sourceAsset.Folder;
                while (folder != null)
                {
                    m_setCachedUnselectedSourceFolders.Remove(folder);
                }
            }
            else
            {
                m_setSelectedSourceAssets.Remove(sourceAsset);

                HrSourceFolder folder = sourceAsset.Folder;
                while (folder != null)
                {
                    m_setCachedSelectedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }
        }

        private void SetSelectedSourceFolder(HrSourceFolder sourceFolder, bool bSelect)
        {
            if (bSelect)
            {
                m_setCachedSelectedSourceFolders.Add(sourceFolder);
                m_setCachedUnselectedSourceFolders.Remove(sourceFolder);

                HrSourceFolder folder = sourceFolder;
                while (folder != null)
                {
                    m_setCachedUnselectedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }
            else
            {
                m_setCachedSelectedSourceFolders.Remove(sourceFolder);
                m_setCachedUnselectedSourceFolders.Add(sourceFolder);

                HrSourceFolder folder = sourceFolder;
                while (folder != null)
                {
                    m_setCachedSelectedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }

            foreach (HrSourceAsset sourceAsset in sourceFolder.GetAssets())
            {
                if (m_bHideAssignedSourceAssets && IsAssignedSourceAsset(sourceAsset))
                {
                    continue;
                }

                SetSelectedSourceAsset(sourceAsset, bSelect);
            }

            foreach (HrSourceFolder subSourceFolder in sourceFolder.GetFolders())
            {
                if (m_bHideAssignedSourceAssets && IsAssignedSourceFolder(subSourceFolder))
                {
                    continue;
                }

                SetSelectedSourceFolder(subSourceFolder, bSelect);
            }
        }

        private bool IsExpandedSourceFolder(HrSourceFolder sourceFolder)
        {
            return m_setExpandedSourceFolders.Contains(sourceFolder);
        }

        private void DrawAssetBundleView()
        {
            int nAssetBundleListRowOnDraw = 0;
            m_vc2AssetBundleViewScroll = EditorGUILayout.BeginScrollView(m_vc2AssetBundleViewScroll);
            {
                DrawAssetBundleFolder(m_assetBundleRoot, ref nAssetBundleListRowOnDraw);
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawAssetBundleFolder(HrAssetBundleFolder assetBundleFolder, ref int nAssetBundleListRowOnDraw)
        {
            bool bExpand = IsExpandedAssetBundleFolder(assetBundleFolder);
            EditorGUILayout.BeginHorizontal();
            {
                if (bExpand != EditorGUI.Foldout(new Rect(18f + 14f * assetBundleFolder.Depth, 20f * nAssetBundleListRowOnDraw + 2f, int.MaxValue, 14f), bExpand, string.Empty, true))
                {
                    bExpand = !bExpand;
                    SetExpandedAssetBundleFolder(assetBundleFolder, bExpand);
                }

                GUI.DrawTexture(new Rect(32f + 14f * assetBundleFolder.Depth, 20f * nAssetBundleListRowOnDraw + 1f, 16f, 16f), HrAssetBundleFolder.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(40f + 14f * assetBundleFolder.Depth), GUILayout.Height(18f));
                EditorGUILayout.LabelField(assetBundleFolder.Name);

            }
            EditorGUILayout.EndHorizontal();

            ++nAssetBundleListRowOnDraw;

            if (bExpand)
            {
                foreach (HrAssetBundleFolder subAssetbundleFolder in assetBundleFolder.GetFolders())
                {
                    DrawAssetBundleFolder(subAssetbundleFolder, ref nAssetBundleListRowOnDraw);
                }
                foreach (HrAssetBundleItem assetBundleItem in assetBundleFolder.GetItems())
                {
                    DrawAssetBundleItem(assetBundleItem, ref nAssetBundleListRowOnDraw);
                }
            }
        }

        private void DrawAssetBundleItem(HrAssetBundleItem assetBundleItem, ref int nAssetBundleListRowOnDraw)
        {
            EditorGUILayout.BeginHorizontal();
            {
                string strTitle = assetBundleItem.Name;
                if (assetBundleItem.AssetBundle.Packed)
                {
                    strTitle = "[Packed]" + strTitle;
                }
                float fEmptySpace = position.width;
                if (EditorGUILayout.Toggle(false, GUILayout.Width(fEmptySpace - 12f)))
                {

                }
                else
                {

                }
                GUILayout.Space(-fEmptySpace + 24f);
                GUI.DrawTexture(new Rect(32f + 14f * assetBundleItem.Depth, 20f * nAssetBundleListRowOnDraw + 1f, 16f, 16f), assetBundleItem.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(26f + 14f * assetBundleItem.Depth), GUILayout.Height(18f));
                EditorGUILayout.LabelField(strTitle);
            }
            EditorGUILayout.EndHorizontal();
            nAssetBundleListRowOnDraw++;
        }

        /// <summary>
        /// 当前目录是否已经展开
        /// </summary>
        /// <param name="assetBundleFolder"></param>
        /// <returns></returns>
        private bool IsExpandedAssetBundleFolder(HrAssetBundleFolder assetBundleFolder)
        {
            return m_setExpandedAssetBundleFolderNames.Contains(assetBundleFolder.FromRootPath);
        }

        /// <summary>
        /// 设置是否展开目录，展开的话就放在容器中，不展开就移除掉 todo
        /// </summary>
        /// <param name="assetBundleFolder"></param>
        /// <param name="bExpand"></param>
        private void SetExpandedAssetBundleFolder(HrAssetBundleFolder assetBundleFolder, bool bExpand)
        {
            if (bExpand)
            {
                m_setExpandedAssetBundleFolderNames.Add(assetBundleFolder.FromRootPath);
            }
            else
            {
                m_setExpandedAssetBundleFolderNames.Remove(assetBundleFolder.FromRootPath);
            }
        }

        private void DrawAssetBundleListMenu()
        {
             DrawAssetBundleMenuAdd();
        }

        private void DrawAssetBundleMenuAdd()
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
                        m_strInputAssetBundleVariant = EditorGUILayout.TextField(m_strInputAssetBundleVariant, GUILayout.Width(80f));
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Add AssetBundle", GUILayout.Width(130), GUILayout.Height(30)))
                        {
                            CancelFocus();

                            if (string.IsNullOrEmpty(m_strInputAssetBundleName))
                            {
                                EditorUtility.DisplayDialog("Warning", "AssetBunle name should not be empty!", "OK");
                            }
                            else
                            {
                                EditorUtility.DisplayProgressBar("Add AssetBundle", "Processing...", 0f);

                                AddAssetBundle(m_strInputAssetBundleName, m_strInputAssetBundleVariant, true);

                                EditorUtility.ClearProgressBar();
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        private void AddAssetBundle(string strAssetBundleName, string strAssetBundleVariant, bool bRefresh)
        {
            if (strAssetBundleVariant == string.Empty)
            {
                strAssetBundleVariant = null;
            }

            string strAssetBundleFullName = GetAssetBundleFullName(strAssetBundleName, strAssetBundleVariant);
            if (m_editorControler.AddAssetBundle(strAssetBundleName, strAssetBundleVariant, HrAssetBundleLoadType.LoadFromFile, false))
            {
                if (bRefresh)
                {
                    RefreshAssetBundleTree();
                }
                Debug.Log(string.Format("Add AssetBundle '{0}' success.", strAssetBundleFullName));
            }
            else
            {
                Debug.LogWarning(string.Format("Add AssetBundle '{0}' failure", strAssetBundleFullName));
            }

        }

        /// <summary>
        /// AssetBundle的完整名称，和Unity打出来的AssetBundle一致 name.variant
        /// </summary>
        /// <param name="strAssetBundleName"></param>
        /// <param name="strAssetBundleVariant"></param>
        /// <returns></returns>
        private string GetAssetBundleFullName(string strAssetBundleName, string strAssetBundleVariant)
        {
            return m_strInputAssetBundleVariant != null ? string.Format("{0}.{1}", strAssetBundleName, strAssetBundleVariant) : strAssetBundleName;
        }

        private void CancelFocus()
        {
            GUI.FocusControl(null);
        }

        /// <summary>
        /// 根据路径字符串创建响应路径
        /// </summary>
        private void RefreshAssetBundleTree()
        {
            m_assetBundleRoot.Clear();

            HrAssetBundle[] assetBundleArr = m_editorControler.GetAssetBundles();
            foreach (HrAssetBundle assetBundle in assetBundleArr)
            {
                string[] strSplitePathArr = assetBundle.Name.Split('/');
                HrAssetBundleFolder folder = m_assetBundleRoot;
                for (int i = 0; i < strSplitePathArr.Length - 1; ++i)
                {
                    HrAssetBundleFolder subFolder = folder.GetFolder(strSplitePathArr[i]);
                    folder = subFolder == null ? folder.AddFolder(strSplitePathArr[i]) : subFolder;
                }

                string strAssetBundleFullName = assetBundle.Variant != null ? string.Format("{0}.{1}", strSplitePathArr[strSplitePathArr.Length - 1], assetBundle.Variant) : strSplitePathArr[strSplitePathArr.Length - 1];
                folder.AddItem(strAssetBundleFullName, assetBundle);
            }
        }


    }
}
