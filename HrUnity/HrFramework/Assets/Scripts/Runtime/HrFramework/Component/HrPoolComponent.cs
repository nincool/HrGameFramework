using Hr.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hr
{
    public sealed class HrPoolComponent : HrComponent
    {

        public bool InitSuccess { get; private set; }

        private IPoolManager m_objectPoolManager = null;

        private Transform m_unityObjectRoot;
        protected override void Awake()
        {
            base.Awake();

            m_objectPoolManager = HrGameWorld.Instance.GetModule<IPoolManager>();
            if (m_objectPoolManager != null)
            {
                InitSuccess = true;
            }
            m_unityObjectRoot = new GameObject("ObjectPool").transform;
            m_unityObjectRoot.SetParent(this.transform);
        }

        public List<HrObjectPool> GetAllObjectPools()
        {
            List<HrObjectPool> lisObjectPool = m_objectPoolManager.GetAllObjectPools().Select(o => o as HrObjectPool).ToList();

            return lisObjectPool;
        }

        public List<HrUnityObjectPool> GetAllUnityObjectPools()
        {
            List<HrUnityObjectPool> lisObjectPool = m_objectPoolManager.GetAllUnityObjectPools().Select(o => o as HrUnityObjectPool).ToList();

            return lisObjectPool;
        }

        public void CreateObjectPool<T>(int nCapacity, int nSpawnCount, float fExpireTime)
        {
            m_objectPoolManager.CreateObjectPool<T>(nCapacity, nSpawnCount, fExpireTime);
        }

        public T GetObject<T>()
        {
            return m_objectPoolManager.GetObject<T>();
        }

        public void ReturnObject<T>(T obj)
        {
            m_objectPoolManager.ReturnObject<T>(obj);
        }

        public void CreateUnityObjectPool(int nCapacity, int nSpawnCount, float fExpireTime, GameObject original)
        {
            m_objectPoolManager.CreateUnityObjectPool(nCapacity, nSpawnCount, fExpireTime, original, m_unityObjectRoot);
        }

        public GameObject GetUnityObject(string strName)
        {
            return m_objectPoolManager.GetUnityObject(strName);
        }

        public void ReturnUnityObject(GameObject obj)
        {
            m_objectPoolManager.ReturnUnityObject(obj);
        }
    }
}
