using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.EventSystem
{
    public enum EnumEventType
    {
        #region UI Loading

        EVENT_PRELOADING_PROGRESS = 1001, //预加载资源进度
        
        #endregion

        #region UI LobbyMainView

        EVENT_UI_CLICK_BTN_1 = 2000,
        EVENT_UI_CLICK_BTN_2,
        EVENT_UI_CLICK_BTN_3,

        #endregion

        #region UI TopBar

        EVENT_UI_TOPBAR_CLICK_SHOWMSG_BTN,
        EVENT_UI_TOPBAR_CLICK_RETURN_BTN,
       
        #endregion

        #region UI Panel01

        EVENT_UI_CLOSE_PANEL01,

        #endregion

        #region UI Panel02

        EVENT_UI_CLOSE_PANEL02,

        #endregion
    }

    //public partial class HrEventType
    //{
    //    #region UI Loading

    //    public const int EVENT_PRELOADING_PROGRESS = 1001;  //预加载资源进度

    //    #endregion

    //    #region UI LobbyMainView

    //    public const int EVENT_UI_CLICK_BTN_1 = 2000;
    //    public const int EVENT_UI_CLICK_BTN_2 = 2001;
    //    public const int EVENT_UI_CLICK_BTN_3 = 2002;

    //    #region UI Panel01

    //    public const int EVENT_UI_CLOSE_PANEL01 = 3001;

    //    #endregion

    //    #region UI Panel02

        

    //    #endregion

    //    #region UI 背包

    //    #endregion
    //}

    public class HrEventPreloadProgressEventHandler : HrEventHandlerArgs
    {
        public int TotalResourceCount
        {
            get;
            private set;
        }

        public int LoadedResourceCount
        {
            get;
            private set;
        }

        public HrEventPreloadProgressEventHandler(int nEvent, object data, int nTotalResCount, int nLoadedResCount) : base(nEvent, data)
        {
            TotalResourceCount = nTotalResCount;
            LoadedResourceCount = nLoadedResCount;
        }
    }
}
