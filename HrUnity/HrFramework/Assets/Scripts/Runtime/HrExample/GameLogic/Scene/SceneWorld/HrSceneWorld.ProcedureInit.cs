using Hr.Define;
using Hr.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene.Procedure.HrSceneWorld
{
    public class HrProcedureInit : HrProcedure
    {

        public HrProcedureInit(HrScene owner) : base(owner)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneWorld.HrProcedureInit.OnEnter!");

        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            base.OnUpdate(fElapseSeconds, fRealElapseSeconds);

            HrGameWorld.Instance.UIComponent.AttachUIRoot();
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventUIViewEventHandler(HrEventType.EVENT_UI_SHOW, null, (int)EnumUIType.UITYPE_LOBBY_MAIN_VIEW));
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventUIViewEventHandler(HrEventType.EVENT_UI_SHOW, null, (int)EnumUIType.UITYPE_TOPBAR_VIEW));
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventUIViewEventHandler(HrEventType.EVENT_UI_SHOW, null, (int)EnumUIType.UITYPE_PANEL02_VIEW));
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventUIViewEventHandler(HrEventType.EVENT_UI_HIDE, null, (int)EnumUIType.UITYPE_PANEL02_VIEW));

            m_owner.ChangeState<Procedure.HrSceneWorld.HrProcedureMain>();

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
