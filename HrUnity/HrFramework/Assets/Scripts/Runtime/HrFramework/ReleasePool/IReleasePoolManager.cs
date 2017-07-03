using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.ReleasePool
{
    public interface IReleasePoolManager
    {
        /// <summary>
        /// 添加到释放池
        /// </summary>
        /// <param name="refObj"></param>
        void AddObject(HrReleaseStartegy releaseStartegy);
    }
}