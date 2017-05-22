using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    [CustomEditor(typeof(HrFSMComponent))]
    public class HrFSMComponentInspector : HrInspector
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            HrFSMComponent inspectTarget = (HrFSMComponent)target;

            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.LabelField(string.Format("Init Success:{0}", inspectTarget.InitSuccess));
            }
        }
    }
}
