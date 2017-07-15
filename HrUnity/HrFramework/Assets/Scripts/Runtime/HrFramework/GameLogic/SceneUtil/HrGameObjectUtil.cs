using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrGameObjectUtil
    {

        public static GameObject Instantiate(GameObject original, Transform parent)
        {
            return Instantiate(original, parent, Vector3.zero, Vector3.zero);
        }

        public static GameObject Instantiate(GameObject original)
        {
            GameObject go = Instantiate(original, original.transform.parent, original.transform.localPosition, original.transform.localRotation.eulerAngles);
            go.transform.localScale = original.transform.localScale;

            return go;
        }

        public static GameObject Instantiate(GameObject original, Transform parent, Vector3 localPosition, Vector3 localRocation)
        {
            if (null == original) return null;
            GameObject go = GameObject.Instantiate(original) as GameObject;
            go.name = original.name;
            AddChildToTarget(parent, go.transform);
            go.transform.localPosition = localPosition;
            go.transform.localRotation = Quaternion.Euler(localRocation);

            return go;
        }

        public static GameObject InstantiateUI(GameObject original, Transform parent)
        {
            if (null == original)
                return null;
            GameObject go = GameObject.Instantiate(original) as GameObject;
            var rtTransform = go.GetComponent<RectTransform>();
            AddChildToTarget(parent, rtTransform);
            rtTransform.localPosition = original.transform.localPosition;
            rtTransform.localRotation = original.transform.localRotation;
            rtTransform.localScale = original.transform.localScale;

            return go;
        }

        /// <summary>
        /// 查找子节点
        /// </summary>
        public static Transform FindDeepChild(GameObject _target, string _childName)
        {
            Transform resultTrs = null;
            resultTrs = _target.transform.Find(_childName);
            if (resultTrs == null)
            {
                foreach (Transform trs in _target.transform)
                {
                    resultTrs = HrGameObjectUtil.FindDeepChild(trs.gameObject, _childName);
                    if (resultTrs != null)
                        return resultTrs;
                }
            }
            return resultTrs;
        }

        /// <summary>
        /// 查找子节点脚本
        /// </summary>
        public static T FindDeepChild<T>(GameObject _target, string _childName) where T : Component
        {
            Transform resultTrs = HrGameObjectUtil.FindDeepChild(_target, _childName);
            if (resultTrs != null)
                return resultTrs.gameObject.GetComponent<T>();
            return (T)((object)null);
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        public static void AddChildToTarget(Transform target, Transform child)
        {
            child.SetParent(target);
            child.localScale = Vector3.one;
            child.localPosition = Vector3.zero;
            child.localEulerAngles = Vector3.zero;

            ChangeChildLayer(child, target.gameObject.layer);
        }

        /// <summary>
        /// 修改子节点Layer  NGUITools.SetLayer();
        /// </summary>
        public static void ChangeChildLayer(Transform t, int layer)
        {
            t.gameObject.layer = layer;
            for (int i = 0; i < t.childCount; ++i)
            {
                Transform child = t.GetChild(i);
                child.gameObject.layer = layer;
                ChangeChildLayer(child, layer);
            }
        }
    }
}
