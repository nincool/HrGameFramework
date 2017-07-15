using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hr.UI
{
    public class HrUIAutoCanvasScale : MonoBehaviour
    {
#if UNITY_EDITOR
        private int m_nLastWidth = 0;
        private int m_nLastHeight = 0;
#endif

        private void Awake()
        {
            SetCanvasScaler();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (m_nLastWidth != Screen.width || m_nLastHeight != Screen.height)
            {
                SetCanvasScaler();
                m_nLastHeight = Screen.height;
                m_nLastWidth = Screen.width;
            }
#else
            this.enabled = false;
#endif

        }

        private void SetCanvasScaler()
        {
            CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

            float screenWidthScale = Screen.width / canvasScaler.referenceResolution.x;
            float screenHeightScale = Screen.height / canvasScaler.referenceResolution.y;

            canvasScaler.matchWidthOrHeight = screenWidthScale > screenHeightScale ? 1 : 0;
        }
    }
}
