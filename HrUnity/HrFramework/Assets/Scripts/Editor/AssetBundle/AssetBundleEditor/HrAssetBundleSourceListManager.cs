using Hr.Editor.Hierarchy;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetBundleSourceListManager
    {
        /// <summary>
        /// 默认路径 最好不要设置为Assets，不然资源太多
        /// </summary>
        private const string m_strDefaultSourceAssetRootPath = "Assets/Media";

        /// <summary>
        /// 
        /// </summary>
        private string m_strSourceAssetRootPath;

        /// <summary>
        /// 查找资源类型
        /// </summary>
        private const string m_c_strSourceAssetUnionLabelFiler = "t:Scene t:Prefab t:Shader t:Model t:Material t:Texture t:AudioClip t:AnimationClip t:AnimatorController t:Font t:TextAsset t:ScriptableObject";


        private HrFileHierarchy m_fileHierarchy;

        public string SourceAssetRootPath
        {
            get
            {
                if (m_strSourceAssetRootPath == null)
                {
                    m_strSourceAssetRootPath = m_strDefaultSourceAssetRootPath;

                }
                return m_strSourceAssetRootPath;
            }
            set
            {
                if (m_strSourceAssetRootPath == value)
                {
                    return;
                }
                m_strSourceAssetRootPath = value.Replace('\\', '/');
            }
        }

        public HrFileHierarchy FileHierarchy
        {
            get
            {
                return m_fileHierarchy;
            }
        }

        public HrAssetBundleAssignManager EditorManager
        {
            get;
            private set;
        }


        public HrAssetBundleSourceListManager(HrAssetBundleAssignManager editorManager)
        {
            EditorManager = editorManager;
        }

        public bool Init()
        {
            return true;
        }

        public bool Save()
        {
            return true;
        }

        public bool Load()
        {
            ScanSourceAssetsHierarchy();
            return true;
        }

        private bool ScanSourceAssetsHierarchy()
        {
            m_fileHierarchy = new HrFileHierarchy(SourceAssetRootPath);

            HashSet<string> setGUIDS = new HashSet<string>();
            setGUIDS.UnionWith(AssetDatabase.FindAssets(m_c_strSourceAssetUnionLabelFiler, new string[] { m_strDefaultSourceAssetRootPath }));

            foreach (string strAssetGUID in setGUIDS)
            {
                string strFullPath = AssetDatabase.GUIDToAssetPath(strAssetGUID);
                if (AssetDatabase.IsValidFolder(strFullPath))
                {
                    continue;
                }

                string strAssetPath = strFullPath.Substring(m_strSourceAssetRootPath.Length + 1);
                m_fileHierarchy.MakeHierarchy(strAssetPath, strAssetGUID);
            }

            return true;
        }

        public void OnDrawSourceListHierarchy()
        {
            m_fileHierarchy.DrawHierarchy();
        }

        public HashSet<HrFileItem> GetSelectedFiles()
        {
            HashSet<HrFileItem> setSelectedFiles = new HashSet<HrFileItem>();
            var lisHierarchyFiles = m_fileHierarchy.GetAllFiles();
            foreach (var hierarchyFile in lisHierarchyFiles)
            {
                if (hierarchyFile.Selected)
                    setSelectedFiles.Add(hierarchyFile.FileItem);
            }

            return setSelectedFiles;
        }

        public bool AssignAsset(HrFileItem file, HrAssetBundle assetBundle)
        {
            if (file.AssetBundle != null)
            {
                return false;
            }

            assetBundle.AssignAsset(file, false);

            return true;
        }

        public bool UnAssignAsset(HrFileItem file)
        {
            if (file.AssetBundle != null)
            {
                file.AssetBundle.Unassign(file);

                return true;
            }

            return false;
        }

        public HrFileItem GetFileItem(string strGUID)
        {
            var lisAllFiles = FileHierarchy.GetAllFiles();
            foreach (var hierarchyFile in lisAllFiles)
            {
                if (hierarchyFile.FileItem.Guid == strGUID)
                {
                    return hierarchyFile.FileItem;
                }
            }

            return null;
        }

        public List<HrFileItem> GetAllFileItems()
        {
            List<HrFileItem> lisFiles = new List<HrFileItem>();
            var lisHierarchyFiles = FileHierarchy.GetAllFiles();
            foreach (var hierarchyFile in lisHierarchyFiles)
            {
                lisFiles.Add(hierarchyFile.FileItem);
            }

            return lisFiles;
        }
    }
}
