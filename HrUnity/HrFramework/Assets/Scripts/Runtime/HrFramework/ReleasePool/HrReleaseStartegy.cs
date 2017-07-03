using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.ReleasePool
{
    public enum EnumReleaseStartegy
    {
        RELEASE_WAITFORENDOFFRAME,  //下一帧释放
        RELEASE_WAITFORSECONDS,     //在等待一定时间释放
        RELEASE_SWICHSCENE,         //切换场景释放
        RELEASE_RESIDENT,           //常驻内存，在APP退出释放
    }

    public sealed class HrReleaseStartegy
    {
        /// <summary>
        /// 剩余时间
        /// </summary>
        public float RemainTime
        {
            get;
            set;
        }

        public EnumReleaseStartegy ReleaseStartegy
        {
            get;
            set;
        }

        public bool IsCompleteResponsibility
        {
            get;
            private set;
        }

        private HrRef m_refObj = null;


        public HrReleaseStartegy(HrRef refObj)
        {
            RemainTime = 0;
            ReleaseStartegy = EnumReleaseStartegy.RELEASE_WAITFORENDOFFRAME;
            IsCompleteResponsibility = false;
            m_refObj = refObj;
        }

        public void Update(float fElapseSeconds, float fRealElapseSeconds)
        {
            switch (ReleaseStartegy)
            {
                case EnumReleaseStartegy.RELEASE_WAITFORENDOFFRAME:
                    {
                        Release();
                        break;
                    }
                case EnumReleaseStartegy.RELEASE_WAITFORSECONDS:
                    {
                        RemainTime -= fElapseSeconds;
                        if (RemainTime <= 0)
                        {
                            Release();
                        }
                        break;
                    }
                case EnumReleaseStartegy.RELEASE_SWICHSCENE:
                    break;
                case EnumReleaseStartegy.RELEASE_RESIDENT:
                    break;
            }
        }

        public void OnChangeScene()
        {

        }

        private void Release()
        {
            m_refObj.Release();
            IsCompleteResponsibility = true;
        }
    }

}
