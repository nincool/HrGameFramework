using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.ObjectPool
{
    public interface IObjectPool<T> where T : HrObjectWrapper
    {
        /// <summary>
        /// 对象池名称
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 对象池中的类型
        /// </summary>
        Type ObjectType
        {
            get;
        }

        /// <summary>
        /// 对象池中当前一共多有多少对象
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 正在被使用的对象数量
        /// </summary>
        int WorkCount
        {
            get;
        }

        /// <summary>
        /// 当前没有用到的对象数量
        /// </summary>
        int IdleCount
        {
            get;
        }

        /// <summary>
        /// 对象池的容量
        /// </summary>
        int Capacity
        {
            get;
        }

        /// <summary>
        /// 获取或设置对象池对象过期秒数。
        /// </summary>
        float ExpireTime
        {
            get;
        }

        /// <summary>
        /// 获取空闲对象
        /// </summary>
        /// <returns></returns>
        object GetIdleObject();

        /// <summary>
        /// 回收空闲对象
        /// </summary>
        /// <param name="obj"></param>
        void ReturnObject(object obj);

        /// <summary>
        /// 获取空闲对象Wrapper
        /// </summary>
        /// <returns></returns>
        T GetObjectIdleWrapper();

        /// <summary>
        /// 回收对象Wrapper
        /// </summary>
        /// <param name="obj"></param>
        void ReturnObjectWrapper(T objWrapper);
    }
}
