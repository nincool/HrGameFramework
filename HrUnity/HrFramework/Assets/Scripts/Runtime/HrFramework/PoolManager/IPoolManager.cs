using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.ObjectPool
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

        UnityEngine.Transform UnityObjectRoot
        {
            get;
            set;
        }

        /// <summary>
        /// 获取所有普通对象池 
        /// </summary>
        /// <returns></returns>
        List<IObjectPool<HrObjectWrapper>> GetAllObjectPools();

        /// <summary>
        /// 创建普通对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nCapacity"></param>
        /// <param name="nSpawnCount"></param>
        /// <param name="fExpireTime"></param>
        void CreateObjectPool<T>(int nCapacity, int nSpawnCount, float fExpireTime);

        /// <summary>
        /// 获取普通对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetObject<T>();

        /// <summary>
        /// 回收普通对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        void ReturnObject<T>(T obj);

        /// <summary>
        /// 获取所有Unity对象池
        /// </summary>
        /// <returns></returns>
        List<IObjectPool<HrObjectWrapper>> GetAllUnityObjectPools();

        /// <summary>
        /// 创建Unity对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nCapacity"></param>
        /// <param name="nSpawnCount"></param>
        /// <param name="fExpireTime"></param>
        void CreateUnityObjectPool(int nCapacity, int nSpawnCount, float fExpireTime, GameObject original, Transform parent);

        /// <summary>
        /// 获取Unity对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        GameObject GetUnityObject(string strName);

        /// <summary>
        /// 回收Unity对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        void ReturnUnityObject(GameObject obj);
    }
}
