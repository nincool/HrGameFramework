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

        private SerializedProperty m_strDatatableModule = null;
        private SerializedProperty m_strEventModule = null;
        private SerializedProperty m_strFSMModule = null;
        private SerializedProperty m_strPoolModule = null;
        private SerializedProperty m_strReleasePoolModule = null;
        private SerializedProperty m_strResourceModule = null;
        private SerializedProperty m_strSceneModule = null;
        private SerializedProperty m_strUIModule = null;
        private SerializedProperty m_strInputModule = null;

        private int m_nEntranceLaunchIndex = 0;
        private int m_nEntranceSceneIndex = 0;

        private int m_nDatatableModuleIndex = 0;
        private int m_nEventModuleIndex = 0;
        private int m_nFSMModuleIndex = 0;
        private int m_nPoolModuleIndex = 0;
        private int m_nReleasePoolModuleIndex = 0;
        private int m_nResourceModuleIndex = 0;
        private int m_nSceneModuleIndex = 0;
        private int m_nUIModuleIndex = 0;
        private int m_nInputModuleIndex = 0;

        private void OnEnable()
        {
            m_strGameVersion = serializedObject.FindProperty("m_strGameVersion");
            m_bNeedSleep = serializedObject.FindProperty("m_bNeverSleep");
            m_bRunInBackGround = serializedObject.FindProperty("m_bRunInBackground");
            m_strLaunch = serializedObject.FindProperty("m_strLaunch");
            m_strEntranceScene = serializedObject.FindProperty("m_strEntryScene");

            m_strDatatableModule = serializedObject.FindProperty("m_strDataTableModule");
            m_strEventModule = serializedObject.FindProperty("m_strEventModule");
            m_strFSMModule = serializedObject.FindProperty("m_strFSMModule");
            m_strPoolModule = serializedObject.FindProperty("m_strPoolModule");
            m_strReleasePoolModule = serializedObject.FindProperty("m_strReleasePoolModule");
            m_strResourceModule = serializedObject.FindProperty("m_strResourceModule");
            m_strSceneModule = serializedObject.FindProperty("m_strSceneModule");
            m_strUIModule = serializedObject.FindProperty("m_strUIModule");
            m_strInputModule = serializedObject.FindProperty("m_strInputModule");

            RefreshSelectedItems();
        }

        private void RefreshSelectedItems()
        {
            RefreshSelected<Hr.DataTable.IDataTableManager>(ref m_nDatatableModuleIndex, m_strDatatableModule.stringValue);
            RefreshSelected<Hr.EventSystem.IEventManager>(ref m_nEventModuleIndex, m_strEventModule.stringValue);
            RefreshSelected<Hr.FSM.IFSMManager>(ref m_nFSMModuleIndex, m_strFSMModule.stringValue);
            RefreshSelected<Hr.ObjectPool.IPoolManager>(ref m_nPoolModuleIndex, m_strPoolModule.stringValue);
            RefreshSelected<Hr.ReleasePool.IReleasePoolManager>(ref m_nReleasePoolModuleIndex, m_strReleasePoolModule.stringValue);
            RefreshSelected<Hr.Resource.IResourceManager>(ref m_nResourceModuleIndex, m_strResourceModule.stringValue);
            RefreshSelected<Hr.Scene.ISceneManager>(ref m_nSceneModuleIndex, m_strSceneModule.stringValue);
            RefreshSelected<Hr.UI.IUIManager>(ref m_nUIModuleIndex, m_strUIModule.stringValue);
            RefreshSelected<Hr.Input.IInputManager>(ref m_nInputModuleIndex, m_strInputModule.stringValue);

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
                var eventTypes = HrType.GetTypeNames(typeof(Hr.DataTable.IDataTableManager));
                eventTypes = AddNullToPopupItems(eventTypes);
                int nSelectedDataTableIndex = EditorGUILayout.Popup("Datatable Module", m_nDatatableModuleIndex, eventTypes);
                if (nSelectedDataTableIndex != m_nDatatableModuleIndex)
                {
                    if (!EditorApplication.isPlaying)
                    {
                        m_nDatatableModuleIndex = nSelectedDataTableIndex;
                        m_strDatatableModule.stringValue = eventTypes[m_nDatatableModuleIndex];
                        if (string.IsNullOrEmpty(m_strDatatableModule.stringValue) || m_strDatatableModule.stringValue == "Null")
                        {
                            m_strDatatableModule.stringValue = string.Empty;
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Can not change this!", "ok");
                    }
                }

                eventTypes = HrType.GetTypeNames(typeof(Hr.EventSystem.IEventManager));
                eventTypes = AddNullToPopupItems(eventTypes);
                int nSelectedEventTypeIndex = EditorGUILayout.Popup("Event Module", m_nEventModuleIndex, eventTypes);
                if (nSelectedEventTypeIndex != m_nEventModuleIndex)
                {
                    if (!EditorApplication.isPlaying)
                    {
                        m_nEventModuleIndex = nSelectedEventTypeIndex;
                        m_strEventModule.stringValue = eventTypes[m_nEventModuleIndex];
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

                eventTypes = HrType.GetTypeNames(typeof(Hr.FSM.IFSMManager));
                eventTypes = AddNullToPopupItems(eventTypes);
                int nSelectedFSMTypeIndex = EditorGUILayout.Popup("FSM Module", m_nFSMModuleIndex, eventTypes);
                if (nSelectedFSMTypeIndex != m_nFSMModuleIndex)
                {
                    if (!EditorApplication.isPlaying)
                    {
                        m_nFSMModuleIndex = nSelectedFSMTypeIndex;
                        m_strFSMModule.stringValue = eventTypes[m_nFSMModuleIndex];
                        if (string.IsNullOrEmpty(m_strFSMModule.stringValue) || m_strFSMModule.stringValue == "Null")
                        {
                            m_strFSMModule.stringValue = string.Empty;
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Can not change this!", "ok");
                    }
                }

                eventTypes = HrType.GetTypeNames(typeof(Hr.ObjectPool.IPoolManager));
                eventTypes = AddNullToPopupItems(eventTypes);
                int nSelectedPoolTypeIndex = EditorGUILayout.Popup("Pool Module", m_nPoolModuleIndex, eventTypes);
                if (nSelectedPoolTypeIndex != m_nPoolModuleIndex)
                {
                    if (!EditorApplication.isPlaying)
                    {
                        m_nPoolModuleIndex = nSelectedPoolTypeIndex;
                        m_strPoolModule.stringValue = eventTypes[m_nPoolModuleIndex];
                        if (string.IsNullOrEmpty(m_strPoolModule.stringValue) || m_strPoolModule.stringValue == "Null")
                        {
                            m_strPoolModule.stringValue = string.Empty;
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Can not change this!", "ok");
                    }
                }

                eventTypes = HrType.GetTypeNames(typeof(Hr.ReleasePool.IReleasePoolManager));
                eventTypes = AddNullToPopupItems(eventTypes);
                int nSelectedReleasePoolIndex = EditorGUILayout.Popup("ReleasePool Module", m_nReleasePoolModuleIndex, eventTypes);
                if (nSelectedReleasePoolIndex != m_nReleasePoolModuleIndex)
                {
                    if (!EditorApplication.isPlaying)
                    {
                        m_nReleasePoolModuleIndex = nSelectedReleasePoolIndex;
                        m_strReleasePoolModule.stringValue = eventTypes[m_nReleasePoolModuleIndex];
                        if (string.IsNullOrEmpty(m_strReleasePoolModule.stringValue) || m_strReleasePoolModule.stringValue == "Null")
                        {
                            m_strReleasePoolModule.stringValue = string.Empty;
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Can not change this!", "ok");
                    }
                }

                eventTypes = HrType.GetTypeNames(typeof(Hr.Resource.IResourceManager));
                eventTypes = AddNullToPopupItems(eventTypes);
                int nSelectedResourceTypeIndex = EditorGUILayout.Popup("Resource Module", m_nResourceModuleIndex, eventTypes);
                if (nSelectedResourceTypeIndex != m_nResourceModuleIndex)
                {
                    if (!EditorApplication.isPlaying)
                    {
                        m_nResourceModuleIndex = nSelectedResourceTypeIndex;
                        m_strResourceModule.stringValue = eventTypes[m_nResourceModuleIndex];
                        if (string.IsNullOrEmpty(m_strResourceModule.stringValue) || m_strResourceModule.stringValue == "Null")
                        {
                            m_strResourceModule.stringValue = string.Empty;
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Can not change this!", "ok");
                    }
                }

                eventTypes = HrType.GetTypeNames(typeof(Hr.Scene.ISceneManager));
                eventTypes = AddNullToPopupItems(eventTypes);
                int nSelectedSceneTypeIndex = EditorGUILayout.Popup("Scene Module", m_nSceneModuleIndex, eventTypes);
                if (nSelectedSceneTypeIndex != m_nSceneModuleIndex)
                {
                    if (!EditorApplication.isPlaying)
                    {
                        m_nSceneModuleIndex = nSelectedSceneTypeIndex;
                        m_strSceneModule.stringValue = eventTypes[m_nSceneModuleIndex];
                        if (string.IsNullOrEmpty(m_strSceneModule.stringValue) || m_strSceneModule.stringValue == "Null")
                        {
                            m_strSceneModule.stringValue = string.Empty;
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Can not change this!", "ok");
                    }
                }

                eventTypes = HrType.GetTypeNames(typeof(Hr.UI.IUIManager));
                eventTypes = AddNullToPopupItems(eventTypes);
                int nSelectedUITypeIndex = EditorGUILayout.Popup("UI Module", m_nUIModuleIndex, eventTypes);
                if (nSelectedUITypeIndex != m_nUIModuleIndex)
                {
                    if (!EditorApplication.isPlaying)
                    {
                        m_nUIModuleIndex = nSelectedUITypeIndex;
                        m_strUIModule.stringValue = eventTypes[m_nUIModuleIndex];
                        if (string.IsNullOrEmpty(m_strUIModule.stringValue) || m_strUIModule.stringValue == "Null")
                        {
                            m_strUIModule.stringValue = string.Empty;
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Can not change this!", "ok");
                    }
                }

                eventTypes = HrType.GetTypeNames(typeof(Hr.Input.IInputManager));
                eventTypes = AddNullToPopupItems(eventTypes);
                int nSelectedInputIndex = EditorGUILayout.Popup("Input Module", m_nInputModuleIndex, eventTypes);
                if (nSelectedInputIndex != m_nInputModuleIndex)
                {
                    if (!EditorApplication.isPlaying)
                    {
                        m_nInputModuleIndex = nSelectedInputIndex;
                        m_strInputModule.stringValue = eventTypes[m_nInputModuleIndex];
                        if (string.IsNullOrEmpty(m_strInputModule.stringValue) || m_strInputModule.stringValue == "Null")
                        {
                            m_strInputModule.stringValue = string.Empty;
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
