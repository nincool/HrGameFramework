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
        private SerializedProperty m_strEntryScene = null;


        private void OnEnable()
        {
            m_strGameVersion = serializedObject.FindProperty("m_strGameVersion");
            m_bNeedSleep = serializedObject.FindProperty("m_bNeverSleep");
            m_bRunInBackGround = serializedObject.FindProperty("m_bRunInBackGround");
            m_strEntryScene = serializedObject.FindProperty("m_strEntryScene");

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            HrGameApp targetInspect = (HrGameApp)target;

            m_bNeedSleep.boolValue = EditorGUILayout.PropertyField(m_bNeedSleep);
            EditorGUILayout.PropertyField(m_bRunInBackGround);

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
            string strEntryScene = EditorGUILayout.DelayedTextField("string EntryScene", m_strEntryScene.stringValue);
            if (strEntryScene != m_strEntryScene.stringValue)
            {
                if (EditorApplication.isPlaying)
                {
                    targetInspect.EntryScene = strEntryScene;
                }
                else
                {
                    m_strEntryScene.stringValue = strEntryScene;
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
