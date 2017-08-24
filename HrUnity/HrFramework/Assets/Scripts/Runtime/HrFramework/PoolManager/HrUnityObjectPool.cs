using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.ObjectPool
{
    public class HrUnityObjectPool : IObjectPool<HrObjectWrapper>
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

        /// <summary>
        /// 原始对象
        /// </summary>
        private GameObject m_original;

        /// <summary>
        /// 存放Idle节点
        /// </summary>
        private Transform m_Root;

        private Stack<HrObjectWrapper> m_staIdleWrapper = new Stack<HrObjectWrapper>();
        private Stack<HrObjectWrapper> m_staWorkWrapper = new Stack<HrObjectWrapper>();

        public HrUnityObjectPool(string strName, int nCapacity, int nSpawnCount, float fExpireTime, GameObject original, Transform parent)
        {
            Name = strName;
            ObjectType = typeof(GameObject);
            if (!ObjectType.IsAssignableFrom(typeof(GameObject)))
            {
                throw new HrException(string.Format("UnityObjectPool Type Error! {0}", Name));
            }
            Capacity = nCapacity;
            ExpireTime = fExpireTime;

            m_original = original;

            m_Root = new GameObject(Name).transform;
            m_Root.SetParent(parent);

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
            GameObject unityObj = obj as GameObject;
            if (m_staWorkWrapper.Count > 0)
            {
                var objWrapper = m_staWorkWrapper.Pop();
                objWrapper.ObtainRef(unityObj);
                m_staIdleWrapper.Push(objWrapper);

                unityObj.transform.SetParent(m_Root);
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
                GameObject obj = HrGameObjectUtil.Instantiate(m_original, m_Root);
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
