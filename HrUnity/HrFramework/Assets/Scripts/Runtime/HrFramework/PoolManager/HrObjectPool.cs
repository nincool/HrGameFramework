using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.ObjectPool
{
    public class HrObjectPool : IObjectPool<HrObjectWrapper>
    {

        /// <summary>
        /// 对象池名称
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// 对象池中的类型
        /// </summary>
        public Type ObjectType
        {
            get;
            protected set;
        }

        /// <summary>
        /// 对象池中当前一共多有多少对象
        /// </summary>
        public int Count
        {
            get
            {
                return IdleCount + WorkCount;
            }
        }

        /// <summary>
        /// 正在被使用的对象数量
        /// </summary>
        public int WorkCount
        {
            get
            {
                return m_staWorkWrapper.Count;
            }
        }

        /// <summary>
        /// 当前没有用到的对象数量
        /// </summary>
        public int IdleCount
        {
            get
            {
                return m_staIdleWrapper.Count;
            }
        }

        /// <summary>
        /// 对象池的容量
        /// </summary>
        public int Capacity
        {
            get;
            protected set;
        }

        /// <summary>
        /// 获取或设置对象池对象过期秒数。
        /// </summary>
        public float ExpireTime
        {
            get;
            protected set;
        }

        private Stack<HrObjectWrapper> m_staIdleWrapper = new Stack<HrObjectWrapper>();
        private Stack<HrObjectWrapper> m_staWorkWrapper = new Stack<HrObjectWrapper>();

        public HrObjectPool(string strName, Type type, int nCapacity, int nSpawnCount, float fExpireTime)
        {
            Name = strName;
            ObjectType = type;
            Capacity = nCapacity;
            ExpireTime = fExpireTime;

            nSpawnCount = nSpawnCount > nCapacity ? nCapacity : nSpawnCount;
            if (nSpawnCount > 0)
            {
                var lisOjbects = new List<object>();
                for (var i = 0; i < nSpawnCount; ++i)
                {
                    var idleObj = GetIdleObject();
                    lisOjbects.Add(idleObj);
                }
                foreach (var idleObj in lisOjbects)
                {
                    ReturnObject(idleObj);
                }
            }
        }

        /// <summary>
        /// 获取空闲对象
        /// </summary>
        /// <returns></returns>
        public object GetIdleObject()
        {
            var objWrapper = GetObjectIdleWrapper();
            if (objWrapper != null)
            {
                return objWrapper.GiveupRef();
            }

            return null;
        }

        /// <summary>
        /// 回收空闲对象
        /// </summary>
        /// <param name="obj"></param>
        public void ReturnObject(object obj)
        {
            if (m_staWorkWrapper.Count > 0)
            {
                var objWrapper = m_staWorkWrapper.Pop();
                objWrapper.ObtainRef(obj);
                m_staIdleWrapper.Push(objWrapper);
            }
            else
            {
                HrLogger.LogError(string.Format("can not return a object to objectpool {0}, because the object pool's workwrapper is empty", ObjectType.FullName));
                return;
            }
        }

        public HrObjectWrapper GetObjectIdleWrapper()
        {
            if (m_staIdleWrapper.Count > 0)
            {
                var objectWrapper = m_staIdleWrapper.Pop();
                m_staWorkWrapper.Push(objectWrapper);

                return objectWrapper;
            }

            if (IdleCount + WorkCount < Capacity)
            {
                HrObjectWrapper objWrapper = new HrObjectWrapper();
                object obj = Activator.CreateInstance(ObjectType);
                if (obj == null)
                {
                    throw new HrException(string.Format("object wrapper: can not instance a object! {0}", ObjectType.FullName));
                }
                objWrapper.ObtainRef(obj);
                m_staWorkWrapper.Push(objWrapper);

                return objWrapper;
            }
            else
            {
                return null;
            }
        }

        public void ReturnObjectWrapper(HrObjectWrapper objWrapper)
        {
            m_staIdleWrapper.Push(objWrapper);
        }
    }
}
