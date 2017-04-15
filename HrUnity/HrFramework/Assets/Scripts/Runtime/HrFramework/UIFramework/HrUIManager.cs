using Hr.CommonUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    public class HrUIManager : Singleton<HrUIManager>
    {
        /// <summary>
        /// 存储所有面板资源路径， 从配置文件中读取 TODO 配置文件读取
        /// </summary>
        private Dictionary<string, string> m_dicPanelAssetPath = new Dictionary<string, string>();

        /// <summary>
        /// 存储所有加载过的面板
        /// </summary>
        private Dictionary<string, HrUIPanel> m_dicPanel = new Dictionary<string, HrUIPanel>();

        /// <summary>
        /// 当前活动的面板
        /// </summary>
        private Stack<HrUIPanel> m_staPanel = new Stack<HrUIPanel>();

        public void PushPanel(string strPanelName)
        {
            if (m_staPanel.Count > 0)
            {
                m_staPanel.Peek().OnPause();
            }

        }

        public HrUIPanel GetPanelByName(string strName)
        {
            HrUIPanel panel = m_dicPanel.HrTryGet(strName);
            if (panel == null)
            {
                string strPanelAssetPath = m_dicPanelAssetPath.HrTryGet(strName);
                if (strPanelAssetPath == null)
                {
                    HrLoger.LogError("HrUIManager GetPanelByName Error! PanelName:" + strName);
                    ///TODO 创建面板
                    
                }
            }

            return panel;
        }
        
    }

}
