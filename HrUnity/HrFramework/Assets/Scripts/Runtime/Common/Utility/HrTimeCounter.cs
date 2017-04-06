using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.CommonUtility
{
    public class HrSimpleTimeCounter
    {
        private long mlStartTimeInMiliSeconds = 0;

        public HrSimpleTimeCounter()
        {
            mlStartTimeInMiliSeconds = HrTimeUtils.GetClientTimeInMilliseconds();
        }

        public long GetTimeElapsed()
        {
            return HrTimeUtils.GetClientTimeInMilliseconds() - mlStartTimeInMiliSeconds;
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
