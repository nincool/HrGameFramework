using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetBundleAssignManager
    {
        public HrAssetBundleListManager AssetBundleListManager
        {
            get;
            private set;
        }

        public HrAssetBundleContentManager AssetBundleContentManger
        {
            get;
            private set;
        }

        public HrAssetBundleSourceListManager SourceListManager
        {
            get;
            private set;
        }

        public HrAssetBundleAssignManager()
        {
            SourceListManager = new HrAssetBundleSourceListManager(this);
            AssetBundleContentManger = new HrAssetBundleContentManager(this);
            AssetBundleListManager = new HrAssetBundleListManager(this);

        }

        public bool Init()
        {
            SourceListManager.Init();
            AssetBundleContentManger.Init();
            AssetBundleListManager.Init();
            return true;
        }

        public bool Load()
        {
            SourceListManager.Load();
            AssetBundleListManager.Load();

            return true;
        }

        public bool Save()
        {
            SetAssetBundleName();
            AssetBundleListManager.Save();
            return true;
        }

        /// <summary>
        /// 设置AssetBundle的Name和Variant
        /// </summary>
        private void SetAssetBundleName()
        {
            var lisFileItems = SourceListManager.GetAllFileItems();
            foreach (var fileItem in lisFileItems)
            {
                AssetImporter assetImporter = AssetImporter.GetAtPath(fileItem.Path);
                HrAssetBundle assetBundle = fileItem.AssetBundle;
                
                if (assetBundle == null)
                {
                    assetImporter.SetAssetBundleNameAndVariant(string.Empty, string.Empty);
                    continue;
                }
                string strAssetBundleName = assetBundle.Name;
                string strAssetBundleVaraint = assetBundle.Variant;
                assetImporter.SetAssetBundleNameAndVariant(strAssetBundleName, strAssetBundleVaraint);
            }
            AssetDatabase.Refresh();
        }


    }
}
