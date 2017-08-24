using Hr.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    [CustomEditor(typeof(HrPoolComponent))]
    public sealed class HrPoolComponentInspector : HrInspector
    {
        private List<bool> m_lisObjectPoolSelected = new List<bool>();
        private List<bool> m_lisUnityObjectPoolSelected = new List<bool>();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            HrPoolComponent inspectTarget = (HrPoolComponent)target;

            var lisAllObjectPools = inspectTarget.GetAllObjectPools();
            if (m_lisObjectPoolSelected.Count < lisAllObjectPools.Count)
            {
                for (var i = 0; i < 100; ++i)
                {
                    m_lisObjectPoolSelected.Add(false);
                }
            }

            EditorGUILayout.LabelField(string.Format("ObjectPool:{0}", lisAllObjectPools.Count), EditorStyles.boldLabel);
            GUILayout.Space(2.5f);
            using (new GUILayout.VerticalScope("box"))
            {
                //foreach (var iteAssetFile in lisAllAssetFiles)
                for (var i = 0; i < lisAllObjectPools.Count; ++i)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Space(10f);
                        bool bExpand = EditorGUILayout.Foldout(m_lisObjectPoolSelected[i], string.Format("Pool:{0}", lisAllObjectPools[i].Name));
                        if (bExpand != m_lisObjectPoolSelected[i])
                        {
                            m_lisObjectPoolSelected[i] = bExpand;
                        }
                    }

                    if (m_lisObjectPoolSelected[i])
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            EditorGUILayout.LabelField(string.Format("  Capacity:{0}/{1}/{2}/{3}", lisAllObjectPools[i].WorkCount
                                , lisAllObjectPools[i].IdleCount
                                , lisAllObjectPools[i].Count
                                , lisAllObjectPools[i].Capacity));
                            EditorGUILayout.LabelField(string.Format("  ExpireTime:{0}", lisAllObjectPools[i].ExpireTime));
                        }
                    }
                }
            }

            GUILayout.Space(5.0f);

            var lisAllUnityObjectPools = inspectTarget.GetAllUnityObjectPools();
            if (m_lisUnityObjectPoolSelected.Count < lisAllUnityObjectPools.Count)
            {
                for (var i = 0; i < 100; ++i)
                {
                    m_lisUnityObjectPoolSelected.Add(false);
                }
            }

            EditorGUILayout.LabelField(string.Format("UnityObjectPool:{0}", lisAllUnityObjectPools.Count), EditorStyles.boldLabel);
            GUILayout.Space(2.5f);
            using (new GUILayout.VerticalScope("box"))
            {
                for (var i = 0; i < lisAllUnityObjectPools.Count; ++i)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Space(10f);
                        bool bExpand = EditorGUILayout.Foldout(m_lisUnityObjectPoolSelected[i], string.Format("Pool:{0}", lisAllUnityObjectPools[i].Name));
                        if (bExpand != m_lisUnityObjectPoolSelected[i])
                        {
                            m_lisUnityObjectPoolSelected[i] = bExpand;
                        }
                    }

                    if (m_lisUnityObjectPoolSelected[i])
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            EditorGUILayout.LabelField(string.Format("  Capacity:{0}/{1}/{2}/{3}", lisAllUnityObjectPools[i].WorkCount
                                , lisAllUnityObjectPools[i].IdleCount
                                , lisAllUnityObjectPools[i].Count
                                , lisAllUnityObjectPools[i].Capacity));
                            EditorGUILayout.LabelField(string.Format("  ExpireTime:{0}", lisAllUnityObjectPools[i].ExpireTime));
                        }
                    }
                }
            }
        }
    }
}
