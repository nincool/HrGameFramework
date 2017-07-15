using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Pool
{
    public interface IPoolManager
    {
        /// <summary>
        /// 获取对象池的数量
        /// </summary>
        int PoolCount
        {
            get;
        }


    }
}
