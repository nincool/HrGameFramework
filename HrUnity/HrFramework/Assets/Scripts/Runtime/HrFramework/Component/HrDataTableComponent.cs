using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr.DataTable;

namespace Hr
{
    public class HrDataTableComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private HrDataTableManager m_dataTableManager = null;

        protected override void Awake()
        {
            base.Awake();

            m_dataTableManager = HrGameWorld.Instance.GetModule<HrDataTableManager>();
            if (m_dataTableManager != null)
            {
                InitSuccess = true;
            }
        }
    }
}
