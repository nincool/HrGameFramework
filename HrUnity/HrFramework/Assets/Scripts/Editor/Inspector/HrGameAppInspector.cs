using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hr.Utility;
using System.Linq;
using System;

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

        private SerializedProperty m_strEventModule = null;

        private int m_nEntranceLaunchIndex = 0;
        private int m_nEntranceSceneIndex = 0;

        private int m_nEventManagerIndex = 0;

        private void OnEnable()
        {
            m_strGameVersion = serializedObject.FindProperty("m_strGameVersion");
            m_bNeedSleep = serializedObject.FindProperty("m_bNeverSleep");
            m_bRunInBackGround = serializedObject.FindProperty("m_bRunInBackground");
            m_strLaunch = serializedObject.FindProperty("m_strLaunch");
            m_strEntranceScene = serializedObject.FindProperty("m_strEntryScene");

            m_strEventModule = serializedObject.FindProperty("m_strEventModule");

            RefreshSelectedItems();
        }

        private void RefreshSelectedItems()
        {
            RefreshSelected<Hr.EventSystem.IEventManager>(ref m_nEventManagerIndex, m_strEventModule.stringValue);

            RefreshSelected<Hr.Logic.HrLaunch>(ref m_nEntranceLaunchIndex, m_strLaunch.stringValue);
            RefreshSelected<Hr.Scene.HrScene>(ref m_nEntranceSceneIndex, m_strEntranceScene.stringValue);

            serializedObject.ApplyModifiedProperties();
        }

        private void RefreshSelected<T>(ref int nIndex, string strValue)
        {
            var typeNames = HrType.GetTypeNames(typeof(T));
            typeNames = AddNullToPopupItems(typeNames);
            nIndex = typeNames.ToList<string>().IndexOf(strValue);
            if (nIndex < 0)
            {
                nIndex = 0;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            HrGameApp targetInspect = (HrGameApp)target;

            EditorGUILayout.BeginVertical("Box");
            {
                bool runInBackground = EditorGUILayout.Toggle("Run in Background", m_bRunInBackGround.boolValue);
                if (runInBackground != m_bRunInBackGround.boolValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        targetInspect.RunInBackground = runInBackground;
                    }
                    else
                    {
                        m_bRunInBackGround.boolValue = runInBackground;
                    }
                }

                bool neverSleep = EditorGUILayout.Toggle("Never Sleep", m_bNeedSleep.boolValue);
                if (neverSleep != m_bNeedSleep.boolValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        targetInspect.NeverSleep = neverSleep;
                    }
                    else
                    {
                        m_bNeedSleep.boolValue = neverSleep;
                    }
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(20.0f);

            EditorGUILayout.LabelField("Module Select", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("Box");
            {
                var eventTypes = HrType.GetTypeNames(typeof(Hr.EventSystem.IEventManager));
                eventTypes = AddNullToPopupItems(eventTypes);

                int nSelectedEventTypeIndex = EditorGUILayout.Popup("Event Manager", m_nEventManagerIndex, eventTypes);
                if (nSelectedEventTypeIndex != m_nEventManagerIndex)
                {
                    if (!EditorApplication.isPlaying)
                    {
                        m_nEventManagerIndex = nSelectedEventTypeIndex;
                        m_strEventModule.stringValue = eventTypes[m_nEventManagerIndex];
                        if (string.IsNullOrEmpty(m_strEventModule.stringValue) || m_strEventModule.stringValue == "Null")
                        {
                            m_strEventModule.stringValue = string.Empty;
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Can not change this!", "ok");
                    }
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(20.0f);

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

            var launchTypes = HrType.GetTypeNames(typeof(Hr.Logic.HrLaunch));
            launchTypes = AddNullToPopupItems(launchTypes);
            int nSelectedEntranceLaunchIndex = EditorGUILayout.Popup("Entrance Launch", m_nEntranceLaunchIndex, launchTypes);
            if (nSelectedEntranceLaunchIndex != m_nEntranceLaunchIndex)
            {
                if (!EditorApplication.isPlaying && nSelectedEntranceLaunchIndex != 0)
                {
                    m_nEntranceLaunchIndex = nSelectedEntranceLaunchIndex;
                    m_strLaunch.stringValue = launchTypes[nSelectedEntranceLaunchIndex];
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Can not change this!", "ok");
                }
            }

            var sceneTypes = HrType.GetTypeNames(typeof(Hr.Scene.HrScene));
            sceneTypes = AddNullToPopupItems(sceneTypes);
            int nSelectedEntranceSceneIndex = EditorGUILayout.Popup("Entrance Scene", m_nEntranceSceneIndex, sceneTypes.ToArray());
            if (nSelectedEntranceSceneIndex != m_nEntranceSceneIndex)
            {
                if (!EditorApplication.isPlaying)
                {
                    m_nEntranceSceneIndex = nSelectedEntranceSceneIndex;
                    m_strEntranceScene.stringValue = sceneTypes[nSelectedEntranceSceneIndex];
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Can not change this!", "ok");
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

        private string[] AddNullToPopupItems(string[] itemsArr)
        {
            var lisItems = new List<string>();
            lisItems.Add("Null");
            lisItems.AddRange(itemsArr);
            itemsArr = lisItems.ToArray();

            return itemsArr;
        }
    }

}
