using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrSimpleTimeCounter
    {
        private long m_nStartTimeInMiliSeconds = 0;

        public HrSimpleTimeCounter()
        {
            m_nStartTimeInMiliSeconds = HrTimeUtils.GetClientTimeInMilliseconds();
        }

        public long GetTimeElapsed()
        {
            return HrTimeUtils.GetClientTimeInMilliseconds() - m_nStartTimeInMiliSeconds;
        }
    }

    public class HrTimeCounter
    {
        public void Reset()
        {

        }

        public void Start()
        {

        }

        public void Pause()
        {

        }

        public void Update()
        {

        }
    }

}
