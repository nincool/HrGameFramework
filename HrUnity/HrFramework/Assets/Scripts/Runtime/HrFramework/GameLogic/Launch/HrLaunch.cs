using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Logic
{
    public abstract class HrLaunch : ILaunch
    {
        

        public virtual void Initialize()
        {
            AddScenes();
        }

        public abstract void AddScenes();

    }

}
