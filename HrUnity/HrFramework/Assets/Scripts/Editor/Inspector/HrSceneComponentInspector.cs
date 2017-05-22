using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hr.Editor
{
    [CustomEditor(typeof(HrSceneComponent))]
    public class HrSceneComponentInspector : HrInspector
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            HrSceneComponent inspectTarget = (HrSceneComponent)target;

            GUILayout.Label("RunningScene:" + inspectTarget.GetRunningSceneName());
        }
    }

}
