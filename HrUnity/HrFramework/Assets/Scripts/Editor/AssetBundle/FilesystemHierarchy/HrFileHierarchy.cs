using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor.Hierarchy
{
    public sealed class HrFileHierarchy
    {
        private HrFileHierarchyFolder m_fileRoot;

        private bool m_bAssignedFilesVisible = true;
        /// <summary>
        /// 绘制文件路径滚动条位置
        /// </summary>
        private Vector2 m_vc2SourceAssetsViewScroll = Vector2.zero;
        public HrFileHierarchyFolder RootFile
        {
            get { return m_fileRoot; }
            set { m_fileRoot = value; }
        }

        public bool AssignedFilesVisible
        {
            get
            {
                return m_bAssignedFilesVisible;
            }
            set
            {
                m_bAssignedFilesVisible = value;
                RefreshHierarchy();
            }
        }

        public HrFileHierarchy(string strRootPath)
        {
            HrFileFolder folder = new HrFileFolder(strRootPath, null);
            m_fileRoot = new HrFileHierarchyFolder(folder);
        }

        public void MakeHierarchy(string strPath, string strGUID)
        {
            string[] strFolderNameArr = strPath.Split('/');
            HrFileHierarchyFolder folder = m_fileRoot;
            for (int i = 0; i < strFolderNameArr.Length - 1; ++i)
            {
                HrFileHierarchyFolder subFolder = folder.GetFileHierarchyFolder(strFolderNameArr[i]);
                if (subFolder == null)
                {
                    subFolder = folder.AddFileHierarchyFolder(strFolderNameArr[i]);
                }
                folder = subFolder;
            }
            
            if (strPath.EndsWith("/"))
            {
                //文件夹
                HrFileHierarchyFolder subFolder = folder.GetFileHierarchyFolder(strFolderNameArr[strFolderNameArr.Length - 1]);
                if (subFolder == null)
                {
                    subFolder = folder.AddFileHierarchyFolder(strFolderNameArr[strFolderNameArr.Length - 1]);
                }
                folder = subFolder;
            }
            else
            {
                //文件
                HrFileHierarchyFile file = folder.GetFileHierarchyFile(strGUID);
                if (file == null)
                {
                    folder.AddFileHierarchyFile(strGUID);
                }
            }

            RefreshHierarchy();
        }

        public void RefreshHierarchy()
        {
            HrFileHierarchyFolder folder = RootFile;

            int nHierarchyRow = 0;

            folder.Row = nHierarchyRow;
            if (folder.Expanded)
            {
                RefreshHierarchyRow(folder, ref nHierarchyRow);
            }
        }

        private void RefreshHierarchyRow(HrFileHierarchyFolder folder, ref int nHierarchyRow)
        {
            var subFolders = folder.GetFileHierarchyFolders();
            foreach (var fileHierarchyFolder in subFolders)
            {
                fileHierarchyFolder.Row = ++nHierarchyRow;
                if (fileHierarchyFolder.Expanded)
                {
                    RefreshHierarchyRow(fileHierarchyFolder, ref nHierarchyRow);
                }
            }
            var subFiles = folder.GetFileHierarchyFiles();
            foreach (var file in subFiles)
            {
                if (!m_bAssignedFilesVisible && file.FileItem.AssetBundle != null)
                {
                    continue;
                }
                file.Row = ++nHierarchyRow;
            }
        }

        public void DrawHierarchy()
        {
            if (m_fileRoot == null)
            {
                return;
            }

            HrFileHierarchyFolder fileFolder = m_fileRoot;

            m_vc2SourceAssetsViewScroll = EditorGUILayout.BeginScrollView(m_vc2SourceAssetsViewScroll);
            {
                DrawFileFolder(fileFolder);
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawFileFolder(HrFileHierarchyFolder folder)
        {
            EditorGUILayout.BeginHorizontal();
            {
                bool bFolderSelected = folder.IsSelected();
                if (bFolderSelected != EditorGUILayout.Toggle(bFolderSelected, GUILayout.Width(12f + 14f * folder.Depth)))
                {
                    folder.SetSelected(!bFolderSelected);
                }

                if (folder.Expanded != EditorGUILayout.Foldout(folder.Expanded, string.Empty))
                {
                    folder.Expanded = !folder.Expanded;
                    RefreshHierarchy();
                }
                GUI.DrawTexture(new Rect(35f + 14f * folder.Depth, 18f * folder.Row , 18f, 18f), HrFileHierarchyFolder.Icon);
                GUILayout.Space(-200f);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(15 * folder.Depth));
                EditorGUILayout.LabelField(string.Format("{0}, {1}, {2}", folder.Name, folder.Row, folder.Depth));
            }
            EditorGUILayout.EndHorizontal();

            if (folder.Expanded)
            {
                foreach (var subFolder in folder.GetFileHierarchyFolders())
                {
                    DrawFileFolder(subFolder);
                }
                foreach (var subFile in folder.GetFileHierarchyFiles())
                {
                    DrawSourceAsset(subFile);
                }
            }
        }

        private void DrawSourceAsset(HrFileHierarchyFile file)
        {
            if (!m_bAssignedFilesVisible && file.FileItem.AssetBundle != null)
            {
                return ;
            }
            EditorGUILayout.BeginHorizontal();
            {

                if (file.Selected != EditorGUILayout.Toggle(file.Selected, GUILayout.Width(28f)))
                {
                    file.Selected = !file.Selected;
                }

                GUI.DrawTexture(new Rect(35f + 14f * file.Depth, 18f * file.Row, 18f, 18f), file.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(14f + 14 * file.Depth));
                if (file.FileItem.AssetBundle == null)
                    EditorGUILayout.LabelField(string.Format("{0} [{1}]", file.Name, file.Depth));
                else
                    EditorGUILayout.LabelField(string.Format("{0}            [{1}] [{2}]", file.Name, file.FileItem.AssetBundle.FullName, file.Depth));
            }
            EditorGUILayout.EndHorizontal();
        }

        public List<HrFileHierarchyFile> GetAllFiles()
        {
            List<HrFileHierarchyFile> lisFiles = new List<HrFileHierarchyFile>();
            GetFolderFiles(RootFile, lisFiles);

            return lisFiles;
        }

        private void GetFolderFiles(HrFileHierarchyFolder hierarchyFolder, List<HrFileHierarchyFile> lisFiles)
        {
            lisFiles.AddRange(hierarchyFolder.GetFileHierarchyFiles());
            var lisSubFolders = hierarchyFolder.GetFileHierarchyFolders();
            foreach (var subFolder in lisSubFolders)
            {
                GetFolderFiles(subFolder, lisFiles);
            }
        }

        public void UnselectAll()
        {
            RootFile.SetSelected(false);
        }
    }
}
