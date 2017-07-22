using Hr.EventSystem;
using Hr.UI;

namespace Hr
{
    public class HrUIComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private IUIManager m_uiManager;
        private IEventManager m_eventManager;

        protected override void Awake()
        {
            base.Awake();

            m_uiManager = HrGameWorld.Instance.GetModule<IUIManager>();
            m_eventManager = HrGameWorld.Instance.GetModule<IEventManager>();
            if (m_uiManager != null && m_eventManager != null)
            {
                InitSuccess = true;
            }
        }

        protected override void Start()
        {
            base.Start();

            m_eventManager.AddHandler(HrEventType.EVENT_SCENE_LOADED_SCENE, HandleLoadedUnityScene);
            m_eventManager.AddHandler(HrEventType.EVENT_SCENE_UNLOAD_SCENE, HandleUnloadUnityScene);
        }

        public void AttachUIRoot()
        {
            m_uiManager.AttachUIRoot();
        }

        public void RegisterUIView(HrUIView uiView)
        {
            m_uiManager.RegisterUIView(uiView);
        }

        public void Reset()
        {
            m_uiManager.Reset();
        }

        private void HandleLoadedUnityScene(object sender, HrEventHandlerArgs args)
        {
            HrLogger.Log("Attach UI Canvas");
            AttachUIRoot();
        }

        private void HandleUnloadUnityScene(object sender, HrEventHandlerArgs args)
        {
            HrLogger.Log("Reset UI Component!");
            Reset();
        }
    }
}
