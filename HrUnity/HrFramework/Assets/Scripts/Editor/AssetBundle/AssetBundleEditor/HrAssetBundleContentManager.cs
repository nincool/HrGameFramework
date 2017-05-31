using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetBundleContentManager
    {
        public HrAssetBundleAssignManager EditorManager
        {
            get;
            private set;
        }

        public HrAssetBundleContentManager(HrAssetBundleAssignManager editorManager)
        {
            EditorManager = editorManager;
        }

        public bool Init()
        {
            return true;
        }

        public bool Load()
        {

            return true;
        }

        public bool Save()
        {

            return true;
        }

    }

}
