using Hr.Define;
using Hr.EventSystem;
using System.Collections;
using System.Collections.Generic;

namespace Hr.UI
{
    public class HrUICheckVersionLogic : HrUILogic
    {
        private HrUILoadingView m_loadingView;

        public HrUICheckVersionLogic()
        {
            ViewType = EnumUIViewType.UI_VIEWTYPE_NORMAL;
            ShowMode = EnumUIShowMode.UI_SHOWMODE_HIDEOTHER;
            ColliderMode = EnumUIColliderMode.UI_COLLIDER_NONE;
        }

        public override void AttachUIView(HrUIView uiView)
        {
            base.AttachUIView(uiView);

            m_loadingView = uiView as HrUILoadingView;
        }

        public override void OnEnter()
        {
            HrGameWorld.Instance.EventComponent.AddHandler((int)EnumEventType.EVENT_PRELOADING_PROGRESS, HandlePreloadingProgress);
        }

        public override void Show()
        {

        }

        public override void OnExit()
        {
            HrGameWorld.Instance.EventComponent.RemoveHandler((int)EnumEventType.EVENT_PRELOADING_PROGRESS, HandlePreloadingProgress);
        }

        private void HandlePreloadingProgress(object sender, HrEventHandlerArgs args)
        {
            HrEventPreloadProgressEventHandler eventArgs = args as HrEventPreloadProgressEventHandler;
            m_loadingView.SetProgressFillAmount(eventArgs.TotalResourceCount, eventArgs.LoadedResourceCount);
        }
    }

}
