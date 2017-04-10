using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hr.CommonUtility
{

    public class HrRef
    {
        private int mnReferenceCount = 0;

        public HrRef()
        {
            mnReferenceCount = 1;
        }

        public int ReferenceCount
        {
            get { return mnReferenceCount; }
        }

        public void Retain()
        {
            Assert.IsTrue(mnReferenceCount > 0);
            ++mnReferenceCount;
        }

        public void Release()
        {
            Assert.IsTrue(mnReferenceCount > 0);
            --mnReferenceCount;
            if (mnReferenceCount == 0)
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
