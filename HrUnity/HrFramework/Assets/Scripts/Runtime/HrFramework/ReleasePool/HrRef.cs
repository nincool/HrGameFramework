using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hr
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
            if (m_nReferenceCount <= 0)
            {
                throw new HrException(string.Format("HrRef Retain Error! RefCount'{0}'", m_nReferenceCount));
            }
            ++m_nReferenceCount;
        }

        public void Release()
        {
            if (m_nReferenceCount <= 0)
            {
                throw new HrException(string.Format("HrRef Release Error! RefCount'{0}'", m_nReferenceCount));
            }
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
