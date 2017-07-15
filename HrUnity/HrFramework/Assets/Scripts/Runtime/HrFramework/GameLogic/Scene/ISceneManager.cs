using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene
{
    public interface ISceneManager
    {
        /// <summary>
        /// 同步加载Unity Scene资源
        /// </summary>
        /// <param name="strSceneName">Scene名称</param>
        void LoadSceneAssetBundleSync(string strAssetBundleFullPath);

        /// <summary>
        /// 从缓存的存有Unity Scene的AssetBundle同步加载Scene
        /// </summary>
        void LoadCachedSceneSync();

        /// <summary>
        /// 加载Unity Scene
        /// </summary>
        /// <param name="nResourceID">Scene对应的资源ID</param>
        void LoadSceneSync(int nResourceID);

        /// <summary>
        /// 卸载当前场景
        /// </summary>
        void UnloadCurrentScene();

        /// <summary>
        /// 注册添加场景
        /// </summary>
        /// <param name="strSceneType"></param>
        void AddScene(string strSceneType);


        HrScene GetRunningScene();

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="strSceneType"></param>
        void SwitchToScene(string strSceneType);


    }
}
