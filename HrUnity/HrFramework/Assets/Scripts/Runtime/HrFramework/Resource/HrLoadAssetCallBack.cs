using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Resource
{
    public class HrLoadAssetCallBack
    {

        private HrDelegateAction<HrAssetFile> m_delegateLoadAssetSuccess = null;

        private HrDelegateAction<string, string> m_delegateLoadAssetFailed = null;

        public HrLoadAssetCallBack(HrDelegateAction<HrAssetFile> loadAssetSuccess, HrDelegateAction<string, string> loadAssetFailed)
        {
            m_delegateLoadAssetSuccess = loadAssetSuccess;
            m_delegateLoadAssetFailed = loadAssetFailed;
        }

        public HrDelegateAction<HrAssetFile> LoadAssetSuccess
        {
            get
            {
                return m_delegateLoadAssetSuccess;
            }
        }

        public HrDelegateAction<string, string> LoadAssetFailed
        {
            get
            {
                return m_delegateLoadAssetFailed;
            }
        }

    }

}
