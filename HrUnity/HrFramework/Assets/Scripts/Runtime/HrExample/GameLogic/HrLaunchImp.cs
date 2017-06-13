﻿using System;
using Hr.Logic;

namespace Hr.Example
{
    public class HrLaunchImp : HrLaunch
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void AddScenes()
        {
            HrGameWorld.Instance.SceneComponent.AddScene("Hr.Scene.HrSceneLaunch");

            HrGameWorld.Instance.EntryScene = "Hr.Scene.HrSceneLaunch";
        }
    }

}
