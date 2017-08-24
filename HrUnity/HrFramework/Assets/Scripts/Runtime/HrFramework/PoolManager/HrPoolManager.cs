using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hr.ObjectPool
{
    public class HrPoolManager : HrModule, IPoolManager
    {
        /// <summary>
        /// 对象池
        /// </summary>
        private Dictionary<Type, IObjectPool<HrObjectWrapper>> m_dicObjectPool = new Dictionary<Type, IObjectPool<HrObjectWrapper>>();

        /// <summary>
        /// UnityObject 对象池
        /// </summary>
        private Dictionary<string, IObjectPool<HrObjectWrapper>> m_dicUnityObjectPool = new Dictionary<string, IObjectPool<HrObjectWrapper>>();

        public int PoolCount
        {
            get;
            private set;
        }

        public Transform UnityObjectRoot
        {
            get;
            set;
        }

        public override void Init()
        {

        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
             
        }

        public override void OnUpdateEndOfFrame(float fElapseSeconds, float fRealElapseSeconds)
        {
        }

        public override void Shutdown()
        {

        }

        public List<IObjectPool<HrObjectWrapper>> GetAllObjectPools()
        {
            return m_dicObjectPool.Values.ToList<IObjectPool<HrObjectWrapper>>();
        }

        public void CreateObjectPool<T>(int nCapacity, int nSpawnCount, float fExpireTime)
        {
            Type t = typeof(T);

            string strTypeName = t.FullName;
            var objectPool = new HrObjectPool(strTypeName, t, nCapacity, nSpawnCount, fExpireTime);
            m_dicObjectPool.Add(t, objectPool);
        }

        public T GetObject<T>()
        {
            var objectPool = GetObjectPool<T>();
            if (objectPool != null)
            {
                return (T)objectPool.GetIdleObject();
            }
            else
            {
                HrLogger.LogError(string.Format("can not find the object pool who contains {0}", typeof(T).FullName));
            }

            return default(T);
        }

        public void ReturnObject<T>(T obj)
        {
            IObjectPool<HrObjectWrapper> objectPool = GetObjectPool<T>();
            if (objectPool != null)
            {
                objectPool.ReturnObject(obj);
            }
            else
            {
                HrLogger.LogError(string.Format("can not find the object pool who contains {0}", typeof(T).FullName));
            }
        }

        /// <summary>
        /// 获取所有Unity对象池
        /// </summary>
        /// <returns></returns>
        public List<IObjectPool<HrObjectWrapper>> GetAllUnityObjectPools()
        {
            return m_dicUnityObjectPool.Values.ToList<IObjectPool<HrObjectWrapper>>();
        }

        /// <summary>
        /// 创建Unity对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nCapacity"></param>
        /// <param name="nSpawnCount"></param>
        /// <param name="fExpireTime"></param>
        public void CreateUnityObjectPool(int nCapacity, int nSpawnCount, float fExpireTime, GameObject original, Transform parent)
        {
            string strTypeName = original.name;
            var objectPool = new HrUnityObjectPool(strTypeName, nCapacity, nSpawnCount, fExpireTime, original, parent);
            m_dicUnityObjectPool.Add(strTypeName, objectPool);
        }

        /// <summary>
        /// 获取Unity对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public GameObject GetUnityObject(string strName)
        {
            var objectPool = GetUnityObjectPool(strName);
            if (objectPool != null)
            {
                return (GameObject)objectPool.GetIdleObject();
            }
            else
            {
                HrLogger.LogError(string.Format("can not find the object pool who contains {0}", strName));
            }

            return null;
        }

        /// <summary>
        /// 回收Unity对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void ReturnUnityObject(GameObject obj)
        {
            var objectPool = GetUnityObjectPool(obj.name);
            if (objectPool != null)
            {
                objectPool.ReturnObject(obj);
            }
            else
            {
                HrLogger.LogError(string.Format("can not find the object pool who contains {0}", obj.name));
            }
        }

        private IObjectPool<HrObjectWrapper> GetObjectPool<T>()
        {
            Type t = typeof(T);

            return m_dicObjectPool.HrTryGet(t);
        }

        private IObjectPool<HrObjectWrapper> GetUnityObjectPool(string strPoolName)
        {
            return m_dicUnityObjectPool.HrTryGet(strPoolName);
        }
    }

}
