using Hr.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Test
{
    public class HrLaunchTest : HrLaunch
    {

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void AddScenes()
        {
            HrGameWorld.Instance.SceneComponent.AddScene<Hr.Test.HrSceneThirdPersonController>();
        }
    }
}
