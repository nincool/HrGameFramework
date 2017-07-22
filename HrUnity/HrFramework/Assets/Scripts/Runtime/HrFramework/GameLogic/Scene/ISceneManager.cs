using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene
{
    public interface ISceneManager
    {
        /// <summary>
        /// 注册添加场景
        /// </summary>
        /// <param name="strSceneType"></param>
        void AddScene(string strSceneType);

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="strSceneType"></param>
        void SwitchToScene(string strSceneType);

        /// <summary>
        /// 获取当前自定义Scene
        /// </summary>
        /// <returns></returns>
        HrScene GetRunningScene();

    }
}
