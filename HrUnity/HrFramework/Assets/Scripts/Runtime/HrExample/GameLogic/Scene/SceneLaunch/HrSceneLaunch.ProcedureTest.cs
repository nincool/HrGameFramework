using Hr.Tests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene.Procedure.HrSceneLaunch
{
    public class HrProcedureTest : HrProcedure
    {
        private Stack<GameObject> m_staTestObject = new Stack<GameObject>();

        private int m_nUpdateCnt = 0;
        private int m_nTestFrame = 0;

        public HrProcedureTest(HrScene owner) : base(owner)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneLaunch.HrProcedureTest.OnEnter!");

            GameObject obj = new GameObject("test");
            HrGameWorld.Instance.PoolComponent.CreateUnityObjectPool(10, 0, -1, obj);
        }

        public void SpawnObject()
        {
            var unityObj = HrGameWorld.Instance.PoolComponent.GetUnityObject("test");
            unityObj.transform.SetParent(null);
            m_staTestObject.Push(unityObj);
        }

        public void ReturnObject()
        {
            if (m_staTestObject.Count > 0)
            {
                var obj = m_staTestObject.Pop();
                HrGameWorld.Instance.PoolComponent.ReturnUnityObject(obj);
            }
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            base.OnUpdate(fElapseSeconds, fRealElapseSeconds);

            if (++m_nUpdateCnt % 10 == 0)
            {
                ++m_nTestFrame;
                if (m_nTestFrame <= 10)
                    SpawnObject();
            }

            if (m_nUpdateCnt % 60 == 0)
            {
                ReturnObject();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }   
}
