using Hr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public enum EnumSceneType
    {
        SCENE_LAUNCH,
        SCENE_HALL,
    }

    public class HrSceneManager 
    {

        private Dictionary<EnumSceneType, HrScene> m_dicScene = new Dictionary<EnumSceneType, HrScene>();

        private HrScene m_runningScene;

        public HrSceneManager()
        {
        }

        public void Init()
        {
            m_dicScene.Add(EnumSceneType.SCENE_LAUNCH, new HrLaunchScene());
            m_dicScene.Add(EnumSceneType.SCENE_HALL, new HrGameHallScene());
        }

        /// <summary>
        /// 在unity切换场景成功之后调用
        /// </summary>
        /// <param name="sceneType"></param>
        public void SwitchToScene(EnumSceneType sceneType)
        {
            var readyToScene = m_dicScene.HrTryGet(sceneType);
            if (readyToScene == null)
            {
                HrLogger.LogError("HrSceneManager SwitchToScene Error! SceneType:" + sceneType);
                return;
            }

            m_runningScene = readyToScene;
            m_runningScene.OnEnter();
            m_runningScene.LoadScene();
        }
    }

}
