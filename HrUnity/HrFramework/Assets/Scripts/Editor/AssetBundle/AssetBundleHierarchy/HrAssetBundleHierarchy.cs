using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetBundleHierarchy
    {
        /// <summary>
        /// 绘制文件路径滚动条位置
        /// </summary>
        private Vector2 m_vc2SourceAssetBundleViewScroll = Vector2.zero;

        private HrAssetBundle m_curSelectedAssetBundle = null;

        public event HrDelegateAction<HrAssetBundle, bool> m_delegateOnToggleAssetBundle;

        public HrAssetBundleFolder AssetBundleFolder
        {
            get;
            private set;
        }

        public HrAssetBundle CurSelectedAssetBundle
        {
            get
            {
                return m_curSelectedAssetBundle;
            }
        }

        public HrAssetBundleHierarchy(string strRootFolder)
        {
            AssetBundleFolder = new HrAssetBundleFolder(strRootFolder);
        }

        public void DrawHierarchy()
        {
            if (AssetBundleFolder == null)
            {
                return;
            }

            HrAssetBundleFolder fileFolder = AssetBundleFolder;

            m_vc2SourceAssetBundleViewScroll = EditorGUILayout.BeginScrollView(m_vc2SourceAssetBundleViewScroll);
            {
                DrawFileFolder(fileFolder);
            }
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// 绘制文件层次，先不管展开不展开，一律展开显示
        /// </summary>
        /// <param name="folder"></param>
        private void DrawFileFolder(HrAssetBundleFolder folder)
        {
            var lisAssetBundles = AssetBundleFolder.GetAssetBundles();
            int nRowIndex = 0;
            foreach (var assetBundle in lisAssetBundles)
            {
                EditorGUILayout.BeginHorizontal();
                {

                    bool bAssetBundleSelected = assetBundle.Selected;
                    if (bAssetBundleSelected != EditorGUILayout.Toggle(bAssetBundleSelected, GUILayout.Width(12f + 14f)))
                    {
                        if (!bAssetBundleSelected)
                        {
                            SetAllAssetBundleSelected(false);
                            assetBundle.Selected = true;
                            m_curSelectedAssetBundle = assetBundle;
                            if (this.m_delegateOnToggleAssetBundle != null)
                            {
                                this.m_delegateOnToggleAssetBundle(assetBundle, true);
                            }
                        }
                        else
                        {
                            SetAllAssetBundleSelected(false);
                            m_curSelectedAssetBundle = null;

                            if (this.m_delegateOnToggleAssetBundle != null)
                            {
                                this.m_delegateOnToggleAssetBundle(assetBundle, false);
                            }
                        }
                    }

                    GUI.DrawTexture(new Rect(18f, 18f * nRowIndex, 18f, 18f), assetBundle.Icon);
                    EditorGUILayout.LabelField(assetBundle.FullName);

                    ++nRowIndex;
                }
                EditorGUILayout.EndHorizontal();
            }

        }

        public void SetAllAssetBundleSelected(bool bSelected)
        {
            var lisAssetBundles = AssetBundleFolder.GetAssetBundles();
            foreach (var assetBundle in lisAssetBundles)
            {
                assetBundle.Selected = bSelected;
            }
        }

        public HrAssetBundle GetAssetBundle(string strAssetBundleFullName)
        {
            return AssetBundleFolder.GetAssetBundle(strAssetBundleFullName);
        }

        public bool RemoveAssetBundle(string strAssetBundleFullName)
        {
            if (strAssetBundleFullName == CurSelectedAssetBundle.FullName)
            {
                m_curSelectedAssetBundle = null;
                SetAllAssetBundleSelected(false);
                if (this.m_delegateOnToggleAssetBundle != null)
                {
                    this.m_delegateOnToggleAssetBundle(CurSelectedAssetBundle, false);
                }
            }
            return AssetBundleFolder.RemoveAssetBundle(strAssetBundleFullName);
        }

    }
}
