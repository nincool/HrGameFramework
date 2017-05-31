using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetUtil
    {
        public static bool IsAssetExisted(string strGUID)
        {
            string strFullPath = AssetDatabase.GUIDToAssetPath(strGUID);
            return !string.IsNullOrEmpty(strFullPath);
        }

        public static bool IsValidFolder(string strGUID)
        {
            string strFullPath = AssetDatabase.GUIDToAssetPath(strGUID);
            return AssetDatabase.IsValidFolder(strFullPath);
        }
    }
}
