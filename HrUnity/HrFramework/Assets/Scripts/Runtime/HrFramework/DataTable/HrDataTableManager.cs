using Hr.Resource;

namespace Hr.DataTable
{
    public class HrDataTableManager : HrModule, IDataTableManager
    {
        private IResourceManager m_resourceManager;

        public void SetResourceManager(IResourceManager resourceManager)
        {
            m_resourceManager = resourceManager;
        }

        public override void Init()
        {
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
        }


    }

}
