using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Resource
{
    public interface IResourceManager
    {
        /// <summary>
        /// 加载资源配置，在资源配置更新为最新之后加载
        /// </summary>
        void LoadAssetsConfig();

        /// <summary>
        /// 加载配置表
        /// </summary>
        /// <param name="strDataTableName"></param>
        /// <param name="loadAssetCallBack"></param>
        void LoadDataTable(string strDataTableName, HrLoadResourceCallBack loadResourceCallBack);

        void LoadResourceSync(int nID, HrLoadResourceCallBack loadResourceCallBack);

        void LoadResourceSync(string strResourceName, HrLoadResourceCallBack loadResourceCallBack);

        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        /// <param name="strAssetBundleName"></param>
        void LoadAssetBundleSync(string strAssetBundleName, HrLoadAssetCallBack loadAssetCallBack);

        /// <summary>
        /// 异步加载AssetBundle
        /// </summary>
        /// <param name="strAssetBundleName"></param>
        /// <param name="loadAssetCallBack"></param>
        void LoadAssetBundleAsync(string strAssetBundleName, HrLoadAssetCallBack loadAssetCallBack);

        /// <summary>
        /// 主动加载AssetBundle 并不参与维护 必须自己来维护 
        /// </summary>
        /// <param name="strFullPath"></param>
        /// <param name="loadAssetCallBack"></param>
        void LoadAssetBundleWithFullPathSync(string strFullPath, HrLoadAssetCallBack loadAssetCallBack);
        
        /// <summary>
        /// 获取Resource
        /// </summary>
        /// <param name="nID">资源的唯一ID</param>
        /// <returns>返回资源对象，如果加载失败为null</returns>
        HrResource GetResource(int nID);

        /// <summary>
        /// 获取Resource
        /// </summary>
        /// <param name="strResourceName">资源的路径名称</param>
        /// <returns>返回资源对象，如果加载失败为null</returns>
        HrResource GetResource(string strResourceName);
    }
}
