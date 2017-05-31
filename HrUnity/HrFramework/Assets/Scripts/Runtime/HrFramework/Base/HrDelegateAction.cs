﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public delegate void HrDelegateAction();
    public delegate void HrDelegateAction<in T>(T obj);
    public delegate void HrDelegateAction<in T1, in T2>(T1 arg1, T2 arg2);
    public delegate void HrDelegateAction<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);
    public delegate void HrDelegateAction<in T1, in T2, in T3, in T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    public delegate void HrDelegateAction<in T1, in T2, in T3, in T4, in T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    public delegate void HrDelegateAction<in T1, in T2, in T3, in T4, in T5, in T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

}
