using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene
{
    public interface ISceneManager
    {
        HrScene GetRunningScene();

        void AddScene(string strSceneType);

        void SwitchToScene(string strSceneType);


    }
}
