using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hr.Utility;
using System.Linq;

namespace Hr.Editor
{
    [CustomEditor(typeof(HrGameApp))]
    public class HrGameAppInspector : HrInspector
    {
        private SerializedProperty m_strGameVersion = null;
        private SerializedProperty m_bNeedSleep = null;
        private SerializedProperty m_bRunInBackGround = null;
        private SerializedProperty m_strLaunch = null;
        private SerializedProperty m_strEntranceScene = null;

        private int m_nEntranceLaunchIndex = 0;
        private int m_nEntranceSceneIndex = 0;

        private void OnEnable()
        {
            m_strGameVersion = serializedObject.FindProperty("m_strGameVersion");
            m_bNeedSleep = serializedObject.FindProperty("m_bNeverSleep");
            m_bRunInBackGround = serializedObject.FindProperty("m_bRunInBackground");
            m_strLaunch = serializedObject.FindProperty("m_strLaunch");
            m_strEntranceScene = serializedObject.FindProperty("m_strEntryScene");

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

            var lisLaunchTypes = HrType.GetTypeNames(typeof(Hr.Logic.HrLaunch));
            int nSelectedEntranceLaunchIndex = EditorGUILayout.Popup("Entrance Launch", m_nEntranceLaunchIndex, lisLaunchTypes.ToArray());
            if (nSelectedEntranceLaunchIndex != m_nEntranceLaunchIndex)
            {
                m_nEntranceLaunchIndex = nSelectedEntranceLaunchIndex;
                m_strLaunch.stringValue = lisLaunchTypes[nSelectedEntranceLaunchIndex];
            }

            var lisSceneTypes = HrType.GetTypeNames(typeof(Hr.Scene.HrScene));
            int nSelectedEntranceSceneIndex = EditorGUILayout.Popup("Entrance Scene", m_nEntranceSceneIndex, lisSceneTypes.ToArray());
            if (nSelectedEntranceSceneIndex != m_nEntranceSceneIndex)
            {
                m_nEntranceSceneIndex = nSelectedEntranceSceneIndex;
                m_strEntranceScene.stringValue = lisSceneTypes[nSelectedEntranceSceneIndex];
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
