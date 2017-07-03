using Hr.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene.Procedure.HrSceneLaunch
{
    public class HrProcedureSplash : HrProcedure
    {
        public HrProcedureSplash(HrScene owner) : base(owner)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            HrLogger.Log("HrSceneLaunch.HrProcedureSplash.OnEnter!");
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            base.OnUpdate(fElapseSeconds, fRealElapseSeconds);

            InitUIRoot();

            if (HrEnvironment.IsEditorMode)
            {
                m_owner.ChangeState<HrProcedurePreload>();
            }
            else
            {
                ///TODO 资源解压缩
                m_owner.ChangeState<HrProcedureCheckVersion>();
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

        private void InitUIRoot()
        {
            //在这里可以切入两个状态
            //1.如果首次安装，那么需要解压缩资源 Uncompress
            //2.检测资源并且更新 CheckVersion
            //无论进入上述哪个状态，都需要一个Loading UI界面，所以UI界面必须在进入后面两个状态前初始化 
            //所以，当前这个场景加载UI有两个解决方案
            //1.固定写死UI的资源路径 无论放在AssetBundle还是放在Resource里，都需要写死路径来加载Loading界面
            //2.把资源更新的UI放在场景中

            ///这里先把UIROOT放在启动场景中
            HrGameWorld.Instance.UIComponent.AttachUIRoot();

        }

    }
}
