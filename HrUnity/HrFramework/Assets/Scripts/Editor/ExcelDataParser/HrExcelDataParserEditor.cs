using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Hr.Utility;


namespace Hr.Editor
{
    public class HrExcelDataParserEditor : EditorWindow
    {
        private Vector2 m_vc2SourceFilesScoll = Vector2.zero;
        private Vector2 m_vc2BinaryFilesScoll = Vector2.zero;

        private HrExcelDataManager m_excelDataParseManager = null;

        [MenuItem("Hr Tools/Excel Parser", false, 102)]
        private static void Opne()
        {
            HrExcelDataParserEditor window = GetWindow<HrExcelDataParserEditor>(true, "Excel Parser", true);
            window.minSize = window.maxSize = new Vector2(666f, 555f);
        }

        private void OnEnable()
        {
            m_excelDataParseManager = new HrExcelDataManager();
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                EditorGUILayout.HelpBox("Waiting to compile", MessageType.Warning);
                return;
            }
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Excel File Folder", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Source Directory", GUILayout.Width(150f));
                        m_excelDataParseManager.SourceDirectory = EditorGUILayout.TextField(m_excelDataParseManager.SourceDirectory);
                        if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                        {
                            BrowseOutputDirectory();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.LabelField("Excel Files:");

                    EditorGUILayout.BeginVertical(GUILayout.Height(100.0f));
                    {
                        m_vc2SourceFilesScoll = EditorGUILayout.BeginScrollView(m_vc2SourceFilesScoll);
                        {
                            if (m_excelDataParseManager.IsValidSourceDirectory)
                            {
                                if (Event.current.type != EventType.DragUpdated
                                    && Event.current.type != EventType.DragExited)
                                {
                                    var strFilePathArr = HrFileUtil.GetAllFilePathsInFolder(m_excelDataParseManager.SourceDirectory, ".xlsx");
                                    foreach (var item in strFilePathArr)
                                    {
                                        EditorGUILayout.LabelField(item);
                                    }
                                }
                            }
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();

                GUILayout.Space(5f);

                EditorGUILayout.LabelField("Binary File Folder", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Direction Directory", GUILayout.Width(150f));
                        m_excelDataParseManager.DestinationDirectory = EditorGUILayout.TextField(m_excelDataParseManager.DestinationDirectory);
                        if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                        {
                            BrowseDestinationDirectory();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.LabelField("Binary Files:");

                    EditorGUILayout.BeginVertical(GUILayout.Height(100.0f));
                    {
                        m_vc2BinaryFilesScoll = EditorGUILayout.BeginScrollView(m_vc2BinaryFilesScoll);
                        {

                            if (m_excelDataParseManager.IsValidDestinationDirectory)
                            {
                                if (Event.current.type != EventType.DragUpdated
                                    && Event.current.type != EventType.DragExited)
                                {
                                    var strFilePathArr = HrFileUtil.GetAllFilePathsInFolder(m_excelDataParseManager.DestinationDirectory);
                                    foreach (var item in strFilePathArr)
                                    {
                                        EditorGUILayout.LabelField(item);
                                    }
                                }
                            }
                        }
                        EditorGUILayout.EndScrollView();
                        if (GUILayout.Button("Clear All Binary Files"))
                        {
                            HrFileUtil.DelectDir(m_excelDataParseManager.DestinationDirectory);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();

                GUILayout.Space(5f);

                EditorGUILayout.LabelField("Data Table Json Path", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Data Table Json Path", GUILayout.Width(160f));
                        m_excelDataParseManager.DataTableJsonPath = EditorGUILayout.TextField(m_excelDataParseManager.DataTableJsonPath);
                        if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                        {
                            BrowseExcelSheetJsonDirectory();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                GUILayout.Space(5f);

                EditorGUILayout.LabelField("Editor File Folder", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Editor Resource Path", GUILayout.Width(160f));
                        m_excelDataParseManager.EditorResourcePath = EditorGUILayout.TextField(m_excelDataParseManager.EditorResourcePath);
                        if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                        {
                            BrowseEditorResourceDirectory();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (GUILayout.Button("Copy To Path"))
                    {
                        m_excelDataParseManager.CopyDataTableToEditorFolder();
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Save", GUILayout.Height(30f)))
                        {
                            m_excelDataParseManager.Save();
                        }
                        if (GUILayout.Button("Translate", GUILayout.Height(30f)))
                        {
                            m_excelDataParseManager.TranslateExcel2Binary();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                GUILayout.Space(5f);

                EditorGUILayout.BeginVertical();
                {

                    if (GUILayout.Button("Test Read"))
                    {
                        m_excelDataParseManager.Read();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        private void BrowseOutputDirectory()
        {
            string directory = EditorUtility.OpenFolderPanel("Select Output Directory", m_excelDataParseManager.SourceDirectory, string.Empty);
            if (!string.IsNullOrEmpty(directory))
            {
                m_excelDataParseManager.SourceDirectory = directory;
            }
        }

        private void BrowseDestinationDirectory()
        {
            string directory = EditorUtility.OpenFolderPanel("Select Direction Directory", m_excelDataParseManager.DestinationDirectory, string.Empty);
            if (!string.IsNullOrEmpty(directory))
            {
                m_excelDataParseManager.DestinationDirectory = directory;
            }
        }

        private void BrowseEditorResourceDirectory()
        {
            string directory = EditorUtility.OpenFolderPanel("Select Direction Directory", m_excelDataParseManager.EditorResourcePath, string.Empty);
            if (!string.IsNullOrEmpty(directory))
            {
                m_excelDataParseManager.EditorResourcePath = directory + "/";
            }
        }

        private void BrowseExcelSheetJsonDirectory()
        {
            string directory = EditorUtility.OpenFolderPanel("Select Direction Directory", m_excelDataParseManager.DataTableJsonPath, string.Empty);
            if (!string.IsNullOrEmpty(directory))
            {
                m_excelDataParseManager.DataTableJsonPath = directory + "/";
            }
        }

    }

}

