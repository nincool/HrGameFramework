using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Resource
{
    public class HrLoadResourceCallBack
    {

        private HrDelegateAction<HrResource> m_delegateLoadResourceSuccess = null;

        private HrDelegateAction<string, string> m_delegateLoadResourceFailed = null;

        public HrLoadResourceCallBack(HrDelegateAction<HrResource> loadResourceSuccess, HrDelegateAction<string, string> loadResourceFailed)
        {
            m_delegateLoadResourceSuccess = loadResourceSuccess;
            m_delegateLoadResourceFailed = loadResourceFailed;
        }

        public HrDelegateAction<HrResource> LoadResourceSuccess
        {
            get
            {
                return m_delegateLoadResourceSuccess;
            }
        }

        public HrDelegateAction<string, string> LoadResourceFailed
        {
            get
            {
                return m_delegateLoadResourceFailed;
            }
        }
    }
}

