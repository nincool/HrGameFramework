using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrLaunchScene : HrScene
    {
        private GameObject m_launchCanvas = null;

        private void Start()
        {
            m_launchCanvas = GameObject.Find("launchCanvas");
            if (m_launchCanvas == null)
            {
                HrLogger.LogError("HrLaunchScene Start launchCanvas is null!");
                return;
            }
        }

        public override void OnEnter()
        {
            HrLogger.Log("HrLanuchScene OnEnter!");

            CreateLoadingUI();
        }

        public override void LoadScene()
        {
            HrLogger.Log("HrLanuchScene LoadScene!");

        }

        public override void OnExit()
        {

        }

        private void CreateLoadingUI()
        {
            HrEventManager.Instance.SendEvent(EnumEvent.EVENT_UI_SHOW, EnumView.VIEW_LAUNCH_LOADING);
        }
    }

}
