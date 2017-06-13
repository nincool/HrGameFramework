﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene
{
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

    public class HrUIRoot
    {
        private GameObject m_uiRoot;
        private Transform[] m_anchorArr = new Transform[(int)EnumUIAnchor.ANCHOR_COUNT];

        public GameObject UIRoot
        {
            get { return m_uiRoot; }
        }

        public Transform GetAnchor(EnumUIAnchor anchor)
        {
            return m_anchorArr[(int)anchor];
        }

        public void InitUIAnchor()
        {
            m_uiRoot = GameObject.Find("CanvasMain");
            if (m_uiRoot == null)
            {
                HrLogger.LogError("HrUIRoot InitUIAnchor uiRoot is null!");
                return;
            }
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_TOPLEFT] = m_uiRoot.transform.FindChild("UIRoot/nodeTopleft");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_TOP] = m_uiRoot.transform.FindChild("UIRoot/nodeTop");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_TOPRIGHT] = m_uiRoot.transform.FindChild("UIRoot/nodeTopright");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_RIGHT] = m_uiRoot.transform.FindChild("UIRoot/nodeRight");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_BUTTOMRIGHT] = m_uiRoot.transform.FindChild("UIRoot/nodeButtomright");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_BUTTOM] = m_uiRoot.transform.FindChild("UIRoot/nodeButtom");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_BUTTOMLEFT] = m_uiRoot.transform.FindChild("UIRoot/nodeButtomleft");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_LEFT] = m_uiRoot.transform.FindChild("UIRoot/nodeLeft");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_CENTER] = m_uiRoot.transform.FindChild("UIRoot/nodeCenter");


        }
    }

    public abstract class HrScene : HrFSMState<HrSceneManager>
    {
        /// <summary>
        /// 用来切换每个Scene的Procedure
        /// </summary>
        protected IFSMStateMachine m_fsmProcedureStateMachine = null;

        public IFSMStateMachine FSMProcedureStateMachine
        {
            get
            {
                return m_fsmProcedureStateMachine;
            }
        }

        public HrScene(HrSceneManager owner) : base(owner)
        {
            m_fsmProcedureStateMachine = HrGameWorld.Instance.FSMComponent.AddFSM<HrScene>(this.GetType().FullName, this) as HrFSMStateMachine<HrScene>;

            AddProcedure();
        }

        public override void OnEnter()
        {
        }

        public override void OnUpdate( float fElapseSeconds, float fRealElapseSeconds)
        {
        }

        public override void OnExit()
        {
            HrGameWorld.Instance.FSMComponent.RemoveFSM(this.GetType().FullName);
        }

        public override void OnDestroy()
        {
        }

        protected abstract void AddProcedure();

        public void ChangeState<StateType>()
        {
            ChangeState(typeof(StateType));
        }

        protected void ChangeState(Type stateType)
        {
            m_fsmProcedureStateMachine.ChangeState(stateType);
        }
    }

}
