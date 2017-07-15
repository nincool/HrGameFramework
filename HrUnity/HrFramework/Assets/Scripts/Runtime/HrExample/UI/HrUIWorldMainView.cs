using Hr.Define;
using Hr.EventSystem;

namespace Hr.UI
{
    public class HrUIWorldMainView : HrUIView
    {
        public override int UIID
        {
            get
            {
                return (int)EnumUIType.UITYPE_LOBBY_MAIN_VIEW;
            }
        }

        public void OnClickButton1()
        {
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventHandlerArgs((int)EnumEventType.EVENT_UI_CLICK_BTN_1, null));
        }

        public void OnClickButton2()
        {
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventHandlerArgs((int)EnumEventType.EVENT_UI_CLICK_BTN_2, null));
        }

        public void OnClickButton3()
        {
            HrGameWorld.Instance.EventComponent.SendEvent(this, new HrEventHandlerArgs((int)EnumEventType.EVENT_UI_CLICK_BTN_3, null));
        }

        public void OnClickButton4()
        {

        }

    }

}
