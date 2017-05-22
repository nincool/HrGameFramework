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
            go.transform.parent = parent;
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
            rtTransform.SetParent(parent);
            rtTransform.localPosition = original.transform.localPosition;
            rtTransform.localRotation = original.transform.localRotation;
            rtTransform.localScale = original.transform.localScale;

            return go;
        }
    }

}
