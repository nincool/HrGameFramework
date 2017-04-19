using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Hr
{
    public class HrUILoadingView : HrUIView
    {
        [SerializeField]
        private Image m_imgProgress = null;

        private void Awake()
        {
            Assert.IsTrue(m_imgProgress != null);

        }

        private void Start()
        {
            HrLogger.Log("HrUILoadingView Start!!!!");
            m_imgProgress.fillAmount = 0.0f;
        }
    }

}
