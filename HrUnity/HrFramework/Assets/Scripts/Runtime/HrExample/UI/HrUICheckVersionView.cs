using Hr.Define;
using UnityEngine;
using UnityEngine.UI;

namespace Hr.UI
{
    public class HrUICheckVersionView : HrUIView
    {
        [SerializeField]
        private Image m_imgProgress;

        public override int UIID
        {
            get
            {
                return (int)EnumUIType.UITYPE_CHECKVIEW_VIEW;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            HrLogger.Log("HrUICehckVersionView OnEnter!");

            m_imgProgress.fillAmount = 0.0f;
        }

        public override void Show()
        {
            base.Show();

        }

        public void SetProgressFillAmount(int nTotalAmount, int nFillAmount)
        {
            if (nTotalAmount <= 0)
            {
                m_imgProgress.fillAmount = 1.0f;
            }
            else
            {
                m_imgProgress.fillAmount = nFillAmount / nTotalAmount * 1.0f;
            }
        }
    }

}
