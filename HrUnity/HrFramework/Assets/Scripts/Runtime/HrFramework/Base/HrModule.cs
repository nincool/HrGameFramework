using System.Collections;
using System.Collections.Generic;

namespace Hr
{
    public abstract class HrModule
    {
        public abstract void Init();

        public abstract void OnUpdate(float fElapseSeconds, float fRealElapseSeconds);
    }
}
