using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Resource
{
    public class HrLoadAssetEventArgs : EventArgs
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 用户数据
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        public HrLoadAssetEventArgs(string strAssetName, object data)
        {
            AssetName = strAssetName;
            UserData = data;
        }
    }

    public sealed class HrLoadAssetSuccessEventArgs : HrLoadAssetEventArgs
    {
        /// <summary>
        /// 持续时间
        /// </summary>
        public float Duration
        {
            get;
            private set;
        }

        public HrLoadAssetSuccessEventArgs(string strAssetName, object data, float fDuration) : base(strAssetName, data)
        {
            Duration = fDuration;
        }
    }

    public sealed class HrLoadAssetFailedEventArgs : HrLoadAssetEventArgs
    {
        public string ErrorMessage
        {
            get;
            private set;
        }

        public HrLoadAssetFailedEventArgs(string strAssetName, object data, string strErrorMessage) : base(strAssetName, data)
        {
            ErrorMessage = strErrorMessage;
        }
    }

    public sealed class HrLoadAssetProgressEventArgs : HrLoadAssetEventArgs
    {
        /// <summary>
        /// 加载资源的进度
        /// </summary>
        public float Progress
        {
            get;
            private set;
        }

        public HrLoadAssetProgressEventArgs(string strAssetName, object data, float fProgress) : base(strAssetName, data)
        {
            Progress = fProgress;
        }
    }

    public sealed class HrLoadAssetEvent
    {
        public event EventHandler<HrLoadAssetSuccessEventArgs> LoadAssetSuccessHandler
        {
            add
            {
                m_eventLoadAssetSuccessHandler += value;
            }
            remove
            {
                m_eventLoadAssetSuccessHandler -= value;
            }
        }

        public event EventHandler<HrLoadAssetFailedEventArgs> LoadAssetFailedHandler
        {
            add
            {
                m_eventLoadAssetFailedHandler += value;
            }
            remove
            {
                m_eventLoadAssetFailedHandler -= value;
            }
        }

        public event EventHandler<HrLoadAssetProgressEventArgs> LoadAssetProgressHandler
        {
            add
            {
                m_eventLoadAssetProgressHandler += value;
            }
            remove
            {
                m_eventLoadAssetProgressHandler -= value;
            }
        }

        private event EventHandler<HrLoadAssetSuccessEventArgs> m_eventLoadAssetSuccessHandler;
        private event EventHandler<HrLoadAssetFailedEventArgs> m_eventLoadAssetFailedHandler;
        private event EventHandler<HrLoadAssetProgressEventArgs> m_eventLoadAssetProgressHandler;

        public void TriggerLoadSuccess(object sender, string strAssetName, object userData, float fDuration)
        {
            m_eventLoadAssetSuccessHandler(sender, new HrLoadAssetSuccessEventArgs(strAssetName, userData, fDuration));
        }

        public void TriggerLoadFailed(object sender, string strAssetName, object userData, string strErrorMessage)
        {
            m_eventLoadAssetFailedHandler(sender, new HrLoadAssetFailedEventArgs(strAssetName, userData, strErrorMessage));
        }

        public void TriggerLoadProgress(object sender, string strAssetName, object userData, float fProgress)
        {
            m_eventLoadAssetProgressHandler(sender, new HrLoadAssetProgressEventArgs(strAssetName, userData, fProgress));
        }
    }

}
