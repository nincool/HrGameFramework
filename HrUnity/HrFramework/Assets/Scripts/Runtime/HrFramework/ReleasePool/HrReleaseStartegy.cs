using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.ReleasePool
{
    public enum EnumReleaseStrategy
    {
        RELEASE_WAITFORENDOFFRAME,  //下一帧释放
        RELEASE_WAITFORSECONDS,     //在等待一定时间释放
        RELEASE_SWICHSCENE,         //切换场景释放
        RELEASE_RESIDENT,           //常驻内存，在APP退出释放
    }

    public sealed class HrReleaseStrategy
    {
        /// <summary>
        /// 剩余时间
        /// </summary>
        public float RemainTime
        {
            get;
            set;
        }

        public EnumReleaseStrategy ReleaseStrategy
        {
            get;
            set;
        }

        public bool IsCompleteResponsibility
        {
            get;
            private set;
        }

        /// <summary>
        /// 自动释放等待帧数，如果是1帧的话，不能保证Update顺序，所以再延迟一帧
        /// </summary>
        private int m_nWaitForEndOfFrames = 2;

        private HrRef m_refObj = null;


        public HrReleaseStrategy(HrRef refObj)
        {
            RemainTime = 60; //默认60秒
            ReleaseStrategy = EnumReleaseStrategy.RELEASE_WAITFORENDOFFRAME;
            IsCompleteResponsibility = false;
            m_nWaitForEndOfFrames = 2;
            m_refObj = refObj;
        }

        public void Update(float fElapseSeconds, float fRealElapseSeconds)
        {
            switch (ReleaseStrategy)
            {
                case EnumReleaseStrategy.RELEASE_WAITFORENDOFFRAME:
                    {
                        if (--m_nWaitForEndOfFrames <= 0)
                        {
                            Release();
                        }
                        break;
                    }
                case EnumReleaseStrategy.RELEASE_WAITFORSECONDS:
                    {
                        RemainTime -= fElapseSeconds;
                        if (RemainTime <= 0)
                        {
                            Release();
                        }
                        break;
                    }
                case EnumReleaseStrategy.RELEASE_SWICHSCENE:
                    break;
                case EnumReleaseStrategy.RELEASE_RESIDENT:
                    break;
            }
        }

        public void OnChangeScene()
        {
            if (ReleaseStrategy == EnumReleaseStrategy.RELEASE_SWICHSCENE)
            {
                Release();
            }
        }

        private void Release()
        {
            m_refObj.Release();
            IsCompleteResponsibility = true;
        }
    }

}
