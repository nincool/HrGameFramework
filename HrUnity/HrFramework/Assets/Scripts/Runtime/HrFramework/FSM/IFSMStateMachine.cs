using System;

namespace Hr
{
    public interface IFSMStateMachine
    {
        string Name
        {
            get;
        }

        void OnUpdate(float fElapseSeconds, float fRealElapseSeconds);

        /// <summary>
        /// 关闭状态机
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 通过类型获取状态
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        IFSMState GetState(Type stateType);

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state"></param>
        void AddState(IFSMState state);

        /// <summary>
        /// 转换状态
        /// </summary>
        /// <param name="stateType"></param>
        void ChangeState(Type stateType);

        /// <summary>
        /// 获取当前的状态
        /// </summary>
        /// <returns></returns>
        IFSMState GetCurrentState();
    }

}
