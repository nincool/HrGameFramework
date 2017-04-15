using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hr.CommonUtility
{

    public class HrRef
    {
        private int m_nReferenceCount = 0;

        public HrRef()
        {
            m_nReferenceCount = 1;
        }

        public int ReferenceCount
        {
            get { return m_nReferenceCount; }
        }

        public void Retain()
        {
            Assert.IsTrue(m_nReferenceCount > 0);
            ++m_nReferenceCount;
        }

        public void Release()
        {
            Assert.IsTrue(m_nReferenceCount > 0);
            --m_nReferenceCount;
            if (m_nReferenceCount == 0)
            {
                //todo release
                ReleaseImp();
            }
        }

        protected virtual void ReleaseImp()
        {
            Assert.IsTrue(false);
        }

    }
}
