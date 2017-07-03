using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr.DataTable;
using Hr.Resource;

namespace Hr
{
    public class HrDataTableComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private IDataTableManager m_dataTableManager = null;



        protected override void Awake()
        {
            base.Awake();

            m_dataTableManager = HrGameWorld.Instance.GetModule<HrDataTableManager>();
            if (m_dataTableManager != null)
            {
                m_dataTableManager.SetResourceManager(HrGameWorld.Instance.GetModule<HrResourceManager>());
                InitSuccess = true;
            }
        }

        public void LoadDataTable(string strDataTable)
        {
            m_dataTableManager.LoadDataTable(strDataTable);
        }

        public HrDataTable GetDataTable(string strDataTable)
        {
            return m_dataTableManager.GetDataTable(strDataTable);
        }
    }
}
