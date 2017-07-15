using Hr.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{ 

    public class HrUIRoot
    {
        private GameObject m_uiRoot;
        private Transform[] m_anchorArr = new Transform[(int)EnumUIAnchor.ANCHOR_COUNT];

        public GameObject UIRoot
        {
            get { return m_uiRoot; }
        }

        public Transform GetAnchor(EnumUIRoot anchor)
        {
            return m_anchorArr[(int)anchor];
        }

        public void AttachUIAnchor()
        {
            m_uiRoot = GameObject.Find("CanvasMain");
            if (m_uiRoot == null)
            {
                HrLogger.LogError("HrUIRoot InitUIAnchor uiRoot is null!");
                return;
            }
            m_anchorArr[(int)EnumUIRoot.UI_ROOT_NORMALROOT] = m_uiRoot.transform.FindChild("UIRoot/NormalRoot");
            m_anchorArr[(int)EnumUIRoot.UI_ROOT_FIXEDROOT] = m_uiRoot.transform.FindChild("UIRoot/FixedRoot");
            m_anchorArr[(int)EnumUIRoot.UI_ROOT_POPUPROOT] = m_uiRoot.transform.FindChild("UIRoot/PopupRoot");
        }

        public void DetachUIAnchor()
        {
            m_uiRoot = null;
            
            for (var i = 0;  i < m_anchorArr.Length; ++i)
            {
                m_anchorArr[i] = null;
            }
        }
    }

}
