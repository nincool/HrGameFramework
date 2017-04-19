using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrScene
    {
        public EnumSceneType SceneType
        {
            get;
            set;
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {

        }

        /// <summary>
        /// 加载并且初始化Scene的基本资源，一般是加载loading的UI，真正的Loading
        /// 动作并不在这里执行
        /// </summary>
        public virtual void LoadScene()
        {
        }


    }

}
