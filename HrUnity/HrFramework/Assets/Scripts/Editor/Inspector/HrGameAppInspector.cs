using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hr.Editor
{
    [CustomEditor(typeof(HrGameApp))]
    public class HrGameAppInspector : HrInspector
    {
        private SerializedProperty m_strGameVersion = null;
        private SerializedProperty m_bNeedSleep = null;
        private SerializedProperty m_bRunInBackGround = null;
        private SerializedProperty m_strLaunch = null;


        private void OnEnable()
        {
            m_strGameVersion = serializedObject.FindProperty("m_strGameVersion");
            m_bNeedSleep = serializedObject.FindProperty("m_bNeverSleep");
            m_bRunInBackGround = serializedObject.FindProperty("m_bRunInBackground");
            m_strLaunch = serializedObject.FindProperty("m_strLaunch");

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            HrGameApp targetInspect = (HrGameApp)target;

            m_bNeedSleep.boolValue = EditorGUILayout.PropertyField(m_bNeedSleep);
            m_bRunInBackGround.boolValue = EditorGUILayout.PropertyField(m_bRunInBackGround);

            string strGameVersion = EditorGUILayout.DelayedTextField("string GameVersion", m_strGameVersion.stringValue);
            if (strGameVersion != m_strGameVersion.stringValue)
            {
                if (EditorApplication.isPlaying)
                {
                    targetInspect.GameVersion = strGameVersion;
                }
                else
                {
                    m_strGameVersion.stringValue = strGameVersion;
                }
            }

            string strLaunch = EditorGUILayout.DelayedTextField("string Launch", m_strLaunch.stringValue);
            if (strLaunch != m_strLaunch.stringValue)
            {
                if (EditorApplication.isPlaying)
                {
                    EditorGUILayout.HelpBox("can not edit!", MessageType.Warning);
                }
                else
                {
                    m_strLaunch.stringValue = strLaunch;
                }
            }

            SaveSerializedData();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();


            Debug.Log("OnCompileComplete !!!!!");
        }

        private void SaveSerializedData()
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

}
