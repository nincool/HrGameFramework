using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    [CustomEditor(typeof(HrEventComponent))]
    public sealed class HrEventComponentInspector : HrInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            HrEventComponent inspectTarget = (HrEventComponent)target;

            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.LabelField(string.Format("Init Success:{0}", inspectTarget.InitSuccess));
            }
        }
    }
}