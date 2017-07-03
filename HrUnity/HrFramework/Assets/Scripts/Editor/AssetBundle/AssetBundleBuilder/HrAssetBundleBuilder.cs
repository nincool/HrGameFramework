//学习Unity Game Framework Homepage: http://gameframework.cn/
// Copyright © 2013-2017 Jiang Yin. All rights reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Hr.Editor
{
    public class HrAssetBundleBuilder : EditorWindow
    {
        private HrAssetBundleController m_builderCtrler = null;

        private bool m_bReadyBuildAssetBundle = false;

        [MenuItem("Hr Tools/AssetBundle Tools/AssetBundle Builder", false, 101)]
        private static void Opne()
        {
            HrAssetBundleBuilder window = GetWindow<HrAssetBundleBuilder>(true, "AssetBundle Builder", true);
            window.minSize = window.maxSize = new Vector2(800f, 700f);
        }

        private void OnEnable()
        {
            m_builderCtrler = new HrAssetBundleController();
            m_bReadyBuildAssetBundle = false;
        }

        private void Update()
        {
            if (m_bReadyBuildAssetBundle)
            {
                m_bReadyBuildAssetBundle = false;
                BuildAssetBundles();
            }
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                EditorGUILayout.HelpBox("Waiting to compile", MessageType.Warning);
                return;
            }
            using (new GUILayout.VerticalScope(GUILayout.Width(position.width), GUILayout.Height(position.height)))
            {
                GUILayout.Space(5f);
                EditorGUILayout.LabelField("Environment Information", EditorStyles.boldLabel);
                using (new EditorGUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Product Name", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_builderCtrler.ProductName);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Company Name", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_builderCtrler.CompanyName);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Game Identifier", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_builderCtrler.GameIdentifier);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Applicable Game Version", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_builderCtrler.ApplicableGameVersion);
                    }
                    EditorGUILayout.EndHorizontal();

                }
                GUILayout.Space(5f);

                #region Build Target
                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUILayout.VerticalScope())
                    {
                        EditorGUILayout.LabelField("Build Target", EditorStyles.boldLabel);

                        using (new EditorGUILayout.VerticalScope("box"))
                        {
                            m_builderCtrler.WindowsSelected = EditorGUILayout.ToggleLeft("Microsoft Windows", m_builderCtrler.WindowsSelected);
                            m_builderCtrler.MacOSXSelected = EditorGUILayout.ToggleLeft("Apple Mac OS X", m_builderCtrler.MacOSXSelected);
                            m_builderCtrler.IOSSelected = EditorGUILayout.ToggleLeft("Apple iPhone/iPad", m_builderCtrler.IOSSelected);
                            m_builderCtrler.AndroidSelected = EditorGUILayout.ToggleLeft("Google Android", m_builderCtrler.AndroidSelected);
                            m_builderCtrler.WindowsStoreSelected = EditorGUILayout.ToggleLeft("Microsoft Windows Store", m_builderCtrler.WindowsStoreSelected);
                        }
                    }
                    using (new EditorGUILayout.VerticalScope())
                    {
                        EditorGUILayout.LabelField("AssetBundle Options", EditorStyles.boldLabel);

                        using (new EditorGUILayout.VerticalScope("box"))
                        {
                            bool uncompressedAssetBundleSelected = EditorGUILayout.ToggleLeft("Uncompressed AssetBundle", m_builderCtrler.UncompressedAssetBundleSelected);
                            if (m_builderCtrler.UncompressedAssetBundleSelected != uncompressedAssetBundleSelected)
                            {
                                m_builderCtrler.UncompressedAssetBundleSelected = uncompressedAssetBundleSelected;
                                if (m_builderCtrler.UncompressedAssetBundleSelected)
                                {
                                    m_builderCtrler.ChunkBasedCompressionSelected = false;
                                }
                            }

                            bool disableWriteTypeTreeSelected = EditorGUILayout.ToggleLeft("Disable Write TypeTree", m_builderCtrler.DisableWriteTypeTreeSelected);
                            if (m_builderCtrler.DisableWriteTypeTreeSelected != disableWriteTypeTreeSelected)
                            {
                                m_builderCtrler.DisableWriteTypeTreeSelected = disableWriteTypeTreeSelected;
                                if (m_builderCtrler.DisableWriteTypeTreeSelected)
                                {
                                    m_builderCtrler.IgnoreTypeTreeChangesSelected = false;
                                }
                            }

                            m_builderCtrler.DeterministicAssetBundleSelected = EditorGUILayout.ToggleLeft("Deterministic AssetBundle", m_builderCtrler.DeterministicAssetBundleSelected);
                            m_builderCtrler.ForceRebuildAssetBundleSelected = EditorGUILayout.ToggleLeft("Force Rebuild AssetBundle", m_builderCtrler.ForceRebuildAssetBundleSelected);

                            bool ignoreTypeTreeChangesSelected = EditorGUILayout.ToggleLeft("Ignore TypeTree Changes", m_builderCtrler.IgnoreTypeTreeChangesSelected);
                            if (m_builderCtrler.IgnoreTypeTreeChangesSelected != ignoreTypeTreeChangesSelected)
                            {
                                m_builderCtrler.IgnoreTypeTreeChangesSelected = ignoreTypeTreeChangesSelected;
                                if (m_builderCtrler.IgnoreTypeTreeChangesSelected)
                                {
                                    m_builderCtrler.DisableWriteTypeTreeSelected = false;
                                }
                            }

                            EditorGUI.BeginDisabledGroup(true);
                            {
                                m_builderCtrler.AppendHashToAssetBundleNameSelected = EditorGUILayout.ToggleLeft("Append Hash To AssetBundle Name", m_builderCtrler.AppendHashToAssetBundleNameSelected);
                            }
                            EditorGUI.EndDisabledGroup();
                            bool chunkBasedCompressionSelected = EditorGUILayout.ToggleLeft("Chunk Based Compression", m_builderCtrler.ChunkBasedCompressionSelected);
                            if (m_builderCtrler.ChunkBasedCompressionSelected != chunkBasedCompressionSelected)
                            {
                                m_builderCtrler.ChunkBasedCompressionSelected = chunkBasedCompressionSelected;
                                if (m_builderCtrler.ChunkBasedCompressionSelected)
                                {
                                    m_builderCtrler.UncompressedAssetBundleSelected = false;
                                }
                            }

                        }
                    }
                }
                #endregion

                string strCompressMessage = string.Empty;
                MessageType compressMessageType = MessageType.None;
                GetCompressMessage(out strCompressMessage, out compressMessageType);
                EditorGUILayout.HelpBox(strCompressMessage, compressMessageType);

                GUILayout.Space(5f);

                #region Build
                EditorGUILayout.LabelField("Build", EditorStyles.boldLabel);

                using (new EditorGUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Internal Resource Version", GUILayout.Width(160f));
                        m_builderCtrler.InternalResourceVersion = EditorGUILayout.IntField(m_builderCtrler.InternalResourceVersion);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Resource Version", GUILayout.Width(160f));
                        GUILayout.Label(string.Format("{0} ({1})", m_builderCtrler.ApplicableGameVersion, m_builderCtrler.InternalResourceVersion.ToString()));
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Output Directory", GUILayout.Width(160f));
                        m_builderCtrler.OutputDirectory = EditorGUILayout.TextField(m_builderCtrler.OutputDirectory);
                        if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                        {
                            BrowseOutputDirectory();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Working Path", GUILayout.Width(160f));
                        GUILayout.Label(m_builderCtrler.WorkingPath);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Assets List Path", GUILayout.Width(160f));
                        GUILayout.Label(m_builderCtrler.AssetsListPath);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("All AssetBundles Path", GUILayout.Width(160f));
                        GUILayout.Label(m_builderCtrler.AllAssetBundlesPath);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Output Packed Path", GUILayout.Width(160f));
                        GUILayout.Label(m_builderCtrler.OutputPackedPath);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Modified AssetBundle Path", GUILayout.Width(160f));
                        GUILayout.Label(m_builderCtrler.ModifiedAssetBundlesPath);
                    }
                    EditorGUILayout.EndHorizontal();                        
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Build Report Path", GUILayout.Width(160f));
                        GUILayout.Label(m_builderCtrler.BuildReportPath);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Editor Resource Path", GUILayout.Width(160f));
                        m_builderCtrler.EditorResourcePath = EditorGUILayout.TextField(m_builderCtrler.EditorResourcePath);
                        if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                        {
                            BrowseEditorResourceDirectory();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                #endregion

                string strBuildMessage = string.Empty;
                MessageType buildMessageType = MessageType.None;
                GetBuildMessage(out strBuildMessage, out buildMessageType);
                EditorGUILayout.HelpBox(strBuildMessage, buildMessageType);
                GUILayout.Space(2f);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Assets List Excel", GUILayout.Width(160f));
                    m_builderCtrler.AssetsConfigExcel = EditorGUILayout.TextField(m_builderCtrler.AssetsConfigExcel);
                    if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                    {
                        BrowseAssetsConfigExcel();
                    }
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2f);

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUI.BeginDisabledGroup(!m_builderCtrler.IsValidOutputDirectory);
                    {
                        if (GUILayout.Button("Start Build AssetBundles"))
                        {
                            m_bReadyBuildAssetBundle = true;
                        }
                    }
                    EditorGUI.EndDisabledGroup();

                    if (GUILayout.Button("Save", GUILayout.Width(80f)))
                    {
                        EditorUtility.DisplayProgressBar("save", "save configuration...", 0f);
                        if (string.IsNullOrEmpty(m_builderCtrler.OutputDirectory))
                        {
                            EditorUtility.DisplayDialog("warning", "outputpaht can not be null!", "ok");
                            EditorUtility.ClearProgressBar();
                            return;
                        }
                        m_builderCtrler.Save();
                        EditorUtility.ClearProgressBar(); 
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Start Zip AssetBundles", GUILayout.Width(160f)))
                    {

                    }
                }
            }
        }

        private void GetCompressMessage(out string message, out MessageType messageType)
        {
            if (m_builderCtrler.ZipSelected)
            {
                if (m_builderCtrler.UncompressedAssetBundleSelected)
                {
                    message = "Compresses AssetBundles with ZIP only. It uses more storage but it's faster when loading the AssetBundles.";
                    messageType = MessageType.Info;
                }
                else if (m_builderCtrler.ChunkBasedCompressionSelected)
                {
                    message = "Compresses AssetBundles with both chunk-based compression and ZIP. Recommended when you use 'AssetBundle.LoadFromFile'.";
                    messageType = MessageType.Info;
                }
                else
                {
                    message = "Compresses AssetBundles with both LZMA and ZIP. Not recommended.";
                    messageType = MessageType.Warning;
                }
            }
            else
            {
                if (m_builderCtrler.UncompressedAssetBundleSelected)
                {
                    message = "Doesn't compress AssetBundles at all. Not recommended.";
                    messageType = MessageType.Warning;
                }
                else if (m_builderCtrler.ChunkBasedCompressionSelected)
                {
                    message = "Compresses AssetBundles with chunk-based compression only. Recommended when you use 'AssetBundle.LoadFromFile'.";
                    messageType = MessageType.Info;
                }
                else
                {
                    message = "Compresses AssetBundles with LZMA only. Recommended when you use 'AssetBundle.LoadFromMemory'.";
                    messageType = MessageType.Info;
                }
            }
        }

        private void BrowseOutputDirectory()
        {
            string directory = EditorUtility.OpenFolderPanel("Select Output Directory", m_builderCtrler.OutputDirectory, string.Empty);
            if (!string.IsNullOrEmpty(directory))
            {
                m_builderCtrler.OutputDirectory = directory;
            }
        }

        private void BrowseEditorResourceDirectory()
        {
            string directory = EditorUtility.OpenFolderPanel("Select Output Directory", m_builderCtrler.EditorResourcePath, string.Empty);
            if (!string.IsNullOrEmpty(directory))
            {
                m_builderCtrler.EditorResourcePath = directory + "/";
            }
        }
        

        private void GetBuildMessage(out string message, out MessageType messageType)
        {
            if (!m_builderCtrler.IsValidOutputDirectory)
            {
                message = "Output directory is invalid.";
                messageType = MessageType.Error;
                return;
            }

            message = string.Empty;
            messageType = MessageType.Info;
            if (Directory.Exists(m_builderCtrler.AssetsListPath))
            {
                message += string.Format("{0} will be overwritten.", m_builderCtrler.AssetsListPath);
                messageType = MessageType.Warning;
            }

            if (Directory.Exists(m_builderCtrler.AllAssetBundlesPath))
            {
                if (message.Length > 0)
                {
                    message += " ";
                }

                message += string.Format("{0} will be overwritten.", m_builderCtrler.AllAssetBundlesPath);
                messageType = MessageType.Warning;
            }

            if (Directory.Exists(m_builderCtrler.OutputPackedPath))
            {
                if (message.Length > 0)
                {
                    message += " ";
                }

                message += string.Format("{0} will be overwritten.", m_builderCtrler.OutputPackedPath);
                messageType = MessageType.Warning;
            }

            if (messageType == MessageType.Warning)
            {
                return;
            }

            message = "Ready to build.";

        }

        private void BuildAssetBundles()
        {
            m_builderCtrler.BuildAssetBundles();
        }

        private void BrowseAssetsConfigExcel()
        {
            string directory = EditorUtility.OpenFilePanel("Select Direction Directory", m_builderCtrler.AssetsConfigExcel, "xlsx");
            if (!string.IsNullOrEmpty(directory))
            {
                m_builderCtrler.AssetsConfigExcel = directory;
            }
        }

    }
}
