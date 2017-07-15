using Hr.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    public class HrUIView : MonoBehaviour, IUIView
    {
        private Transform[] m_anchorArr = new Transform[(int)EnumUIAnchor.ANCHOR_COUNT];

        private static List<Vector3> m_s_lisAnchorPos = null;

        /// <summary>
        /// UI 唯一ID
        /// </summary>
        public virtual int UIID
        {
            get
            {
                return 0;
            }
        }

        public virtual void Awake()
        {
            ///todo 重新调整UI位置
            //CheckAnchors();

            HrGameWorld.Instance.UIComponent.RegisterUIView(this);
        }

        /// <summary>
        /// 首次挂载界面
        /// </summary>
        public virtual void OnEnter()
        {
        }

        /// <summary>
        /// 界面显示
        /// </summary>
        public virtual void Show()
        {
            HrLogger.Log("HrUIView Show!!!!!");
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);
        }

        public virtual void Reshow()
        {
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);
        }

        /// <summary>
        ///Update 
        /// </summary>
        public virtual void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
        }

        public virtual void Freeze()
        {

        }

        public virtual void Unfreeze()
        {

        }

        /// <summary>
        /// 界面隐藏
        /// </summary>
        public virtual void Hide()
        {
            if (this.gameObject.activeSelf)
                this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 界面销毁退出
        /// </summary>
        public virtual void OnExit()
        {
        }

        #region private methods

        #region anchors

        private void CheckAnchors()
        {
            FindAnchorNodes();
            CalculateAnchosPosition();
            AdjustAnchorsPosition();
        }

        private void FindAnchorNodes()
        {
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_TOPLEFT] = this.transform.FindChild("nodeTopleft");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_TOP] = this.transform.FindChild("nodeTop");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_TOPRIGHT] = this.transform.FindChild("nodeTopright");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_RIGHT] = this.transform.FindChild("nodeRight");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_BUTTOMRIGHT] = this.transform.FindChild("nodeButtomright");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_BUTTOM] = this.transform.FindChild("nodeButtom");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_BUTTOMLEFT] = this.transform.FindChild("nodeButtomleft");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_LEFT] = this.transform.FindChild("nodeLeft");
            m_anchorArr[(int)EnumUIAnchor.ANCHOR_CENTER] = this.transform.FindChild("nodeCenter");
        }

        private void CalculateAnchosPosition()
        {
            if (null == m_s_lisAnchorPos || m_s_lisAnchorPos.Count <= 0)
            {
                m_s_lisAnchorPos = new List<Vector3>();


                float fHalfScreenWidth = Screen.width * 0.5f;
                float fHalfScreenHeight = Screen.height * 0.5f;

                var canvasMain = GameObject.Find("CanvasMain");
                if (canvasMain == null)
                {
                    HrLogger.LogError("CalculateAnchosPosition canvasMain is null!");
                    return;
                }


                m_s_lisAnchorPos.Add(new Vector3(-fHalfScreenWidth, fHalfScreenHeight, 0));
                m_s_lisAnchorPos.Add(new Vector3(0, fHalfScreenHeight, 0));
                m_s_lisAnchorPos.Add(new Vector3(fHalfScreenWidth, fHalfScreenHeight, 0));
                m_s_lisAnchorPos.Add(new Vector3(fHalfScreenWidth, 0, 0));
                m_s_lisAnchorPos.Add(new Vector3(fHalfScreenWidth, -fHalfScreenHeight, 0));
                m_s_lisAnchorPos.Add(new Vector3(0, -fHalfScreenHeight, 0));
                m_s_lisAnchorPos.Add(new Vector3(-fHalfScreenWidth, -fHalfScreenHeight, 0));
                m_s_lisAnchorPos.Add(new Vector3(-fHalfScreenWidth, 0, 0));
                m_s_lisAnchorPos.Add(new Vector3(0, 0, 0));
            }
        }

        private void AdjustAnchorsPosition()
        {
            for (var i = 0; i < m_anchorArr.Length; ++i)
            {
                if (null != m_anchorArr[i])
                {
                    m_anchorArr[i].localPosition = m_s_lisAnchorPos[i];
                }
            }
        }

        #endregion

        #endregion

    }
}
