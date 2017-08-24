using System;
using System.Collections.Generic;

namespace Hr.ReleasePool
{
    public class HrReleasePoolManager : HrModule, IReleasePoolManager
    {
        private List<HrReleaseStrategy> m_lisObj = new List<HrReleaseStrategy>();

        #region IReleasePoolManager

        public void AddObject(HrReleaseStrategy ReleaseStrategy)
        {
            m_lisObj.Add(ReleaseStrategy);
        }

        public void OnChangeScene()
        {
            for (var i = 0; i < m_lisObj.Count; ++i)
            {
                m_lisObj[i].OnChangeScene();
            }
        }

        #endregion

        #region HrModule
        public override void Init()
        {

        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            for (var i = 0; i < m_lisObj.Count; ++i)
            {
                m_lisObj[i].Update(fElapseSeconds, fRealElapseSeconds);
                if (m_lisObj[i].IsCompleteResponsibility)
                {
                    m_lisObj.RemoveAt(i--);
                }
            }
        }

        public override void OnUpdateEndOfFrame(float fElapseSeconds, float fRealElapseSeconds)
        {
        }

        public override void Shutdown()
        {

        }
        #endregion


    }
}


