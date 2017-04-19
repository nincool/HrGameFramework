using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    /// <summary>
    /// 挂载场景节点脚本 只为了能够使用协程
    /// </summary>
    public class HrCoroutineBehaviour : MonoBehaviour { }

    public class HrCoroutineManager
    {
      
        private static WeakReference ms_monoComponent;

        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            if (!ms_monoComponent.IsAlive)
            {
                var com = new GameObject("CoroutineManager").AddComponent<HrCoroutineBehaviour>();
                ms_monoComponent = new WeakReference(com);
            }
            return (ms_monoComponent.Target as HrCoroutineBehaviour).StartCoroutine(routine);
        }
    }

}

