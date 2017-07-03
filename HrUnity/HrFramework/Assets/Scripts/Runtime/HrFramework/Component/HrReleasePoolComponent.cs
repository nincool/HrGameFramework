using Hr.ReleasePool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrReleasePoolComponent : HrComponent
    {
        public bool InitSuccess { get; private set; }

        private IReleasePoolManager m_releasePoolManager;

        protected override void Awake()
        {
            base.Awake();

            m_releasePoolManager = HrGameWorld.Instance.GetModule<HrReleasePoolManager>();
            if (m_releasePoolManager != null)
            {
                InitSuccess = true;
            }
        }

        public void AddReleaseStartegyObject(HrReleaseStartegy releaseStartegy)
        {
            m_releasePoolManager.AddObject(releaseStartegy);
        }

    }
}
