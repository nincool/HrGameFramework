using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Define
{
    public enum EnumUIViewType
    {
        UI_VIEWTYPE_NORMAL,     //普通界面 可以作为一个主界面的View 例如MainLobbyUI 
        UI_VIEWTYPE_FIXED,      //固定界面 可以单独挂载的非全屏界面 比如顶部玩家信息 TopBar 并不一定都有 也并不一定都存在
        UI_VIEWTYPE_POPUP,      //弹出界面
    }

    public enum EnumUIShowMode
    {
        UI_SHOWMODE_DONOTHING,  //不做任何事情 一般就是普通界面的显示 比如TopBar LeftBar RightBar 快速开始等等在进入游戏同时显示的UI
        UI_SHOWMODE_HIDEOTHER,  //隐藏其他所有界面 一般用于全局性的窗体 可以覆盖其他的界面，被覆盖的界面一般设为不可见
        UI_SHOWMODE_NEEDBACK,   //可以返回到前界面 一般用在弹出界面(Popup)的管理上，一般这种界面不完全覆盖底层界面，按返回键可以原路返回 加入界面堆栈
    }

    public enum EnumUIColliderMode
    {
        UI_COLLIDER_NONE,           //透明可以穿透
        UI_COLLIDER_MASKWITHOUTBG,  //透明不可以穿透
        UI_COLLIDER_MASKWITHBG      //不透明不可以穿透
    }

    public enum EnumUIRoot
    {
        UI_ROOT_NORMALROOT,
        UI_ROOT_FIXEDROOT,
        UI_ROOT_POPUPROOT,
    }

    public enum EnumUIAnchor
    {
        ANCHOR_TOPLEFT = 0,
        ANCHOR_TOP,
        ANCHOR_TOPRIGHT,
        ANCHOR_RIGHT,
        ANCHOR_BUTTOMRIGHT,
        ANCHOR_BUTTOM,
        ANCHOR_BUTTOMLEFT,
        ANCHOR_LEFT,
        ANCHOR_CENTER,
        ANCHOR_COUNT,
    }



}
