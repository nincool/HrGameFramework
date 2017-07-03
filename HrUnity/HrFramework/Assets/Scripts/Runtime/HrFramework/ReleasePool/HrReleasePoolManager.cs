using System;
using System.Collections.Generic;

namespace Hr.ReleasePool
{
    public class HrReleasePoolManager : HrModule, IReleasePoolManager
    {
        private List<HrReleaseStartegy> m_lisObj = new List<HrReleaseStartegy>();

        #region IReleasePoolManager

        public void AddObject(HrReleaseStartegy releaseStartegy)
        {
            m_lisObj.Add(releaseStartegy);
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

        public override void Shutdown()
        {

        }
        #endregion

        private void OnChangeScene()
        {
            for (var i = 0; i < m_lisObj.Count; ++i)
            {
                m_lisObj[i].OnChangeScene();
            }
        }
    }
}
