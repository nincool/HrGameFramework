using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    /// <summary>
    /// 挂载场景节点脚本 只为了能够使用协程
    ///     new WaitForEndOfFrame;      //等待一帧
    ///     new WaitForFixedUpdate;     //等待一个FixedUpdate（固定时间间隔）
    ///     new WaitForSeconds;         //等待X秒
    ///     new WWW;                    //等待外部资源加载完毕
    /// </summary>
    public class HrCoroutineBehaviour : MonoBehaviour { }


    public class HrCoroutineManager
    {
      
        private static WeakReference ms_monoComponent = null;

        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            if (ms_monoComponent == null || !ms_monoComponent.IsAlive)
            {
                var com = new GameObject("CoroutineManager").AddComponent<HrCoroutineBehaviour>();
                GameObject gameAppObj = GameObject.Find("GameApp");
                if (gameAppObj != null)
                {
                    com.transform.parent = gameAppObj.transform;
                }
                ms_monoComponent = new WeakReference(com);
            }
            return (ms_monoComponent.Target as HrCoroutineBehaviour).StartCoroutine(routine);
        }
    }

}

