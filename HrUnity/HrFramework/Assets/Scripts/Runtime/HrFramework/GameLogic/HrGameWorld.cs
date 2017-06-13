using Hr.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrGameWorld : HrSingleton<HrGameWorld>
    {

        /// <summary>
        /// 游戏组件 持有游戏模块
        /// </summary>
        private readonly LinkedList<HrComponent> m_components = new LinkedList<HrComponent>();

        /// <summary>
        /// 游戏模块 具体的功能实现者
        /// </summary>
        private readonly LinkedList<HrModule> m_modules = new LinkedList<HrModule>();

        /// <summary>
        /// 资源组件
        /// </summary>
        public HrResourceComponent ResourceComponent { get; private set; }

        /// <summary>
        /// 事件管理器组件
        /// </summary>
        public HrEventComponent EventComponent { get; private set; }

        /// <summary>
        /// 状态机组件
        /// </summary>
        public HrFSMComponent FSMComponent { get; private set; }

        /// <summary>
        /// 场景管理器组件
        /// </summary>
        public HrSceneComponent SceneComponent { get; private set; }

        /// <summary>
        /// 组件节点
        /// </summary>
        public Transform ComponentRoot
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏根节点
        /// </summary>
        public Transform GameRoot
        {
            get;
            set;
        }

        /// <summary>
        /// 初始化的场景
        /// </summary>
        public string EntryScene
        {
            get;
            set;
        }

        /// <summary>
        /// 初始化ILaunch
        /// </summary>
        private ILaunch m_launch;

        public void Initialize(string strLaunch)
        {
            InitGameComonent();

            foreach (var module in m_modules)
            {
                module.Init();
            }

            Type t = Type.GetType(strLaunch);
            if (t == null)
            {
                HrLogger.LogError(string.Format("HrGameWorld initialize error! error launch type! [{0}]", strLaunch));
                return;
            }
            m_launch = (ILaunch)Activator.CreateInstance(t);
            if (m_launch != null)
            {
                m_launch.Initialize();
            }
            else
            {
                HrLogger.LogError("create launch error!");
            }
        }

        private void InitGameComonent()
        {
            EventComponent = GetHrComponent<HrEventComponent>();
            FSMComponent = GetHrComponent<HrFSMComponent>();
            ResourceComponent = GetHrComponent<HrResourceComponent>();
            SceneComponent = GetHrComponent<HrSceneComponent>();
        }

        public void StartGame()
        {
            HrLogger.Log("HrGameWorld Start Game EntryScene:" + EntryScene);
            if (!string.IsNullOrEmpty(EntryScene))
            {
                SceneComponent.SwitchToScene(EntryScene);
            }
        }

        #region HrComponent
        private T AddHrComponent<T>() where T : HrComponent
        {
            Type type = typeof(T);

            LinkedListNode<HrComponent> current = m_components.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    HrLogger.LogError(string.Format("Game framework component type '{0}' is already exist.", type.FullName));
                    return (T)current.Value;
                }

                current = current.Next;
            }

            GameObject eventComponent = new GameObject(typeof(T).FullName);
            HrComponent component = eventComponent.AddComponent<T>();
            if (component == null)
            {
                HrLogger.LogError(string.Format("can not add the component '{0}' .", type.FullName));
                return null;
            }
            eventComponent.transform.parent = ComponentRoot;
            m_components.AddLast(component);

            return (T)component;
        }

        private void RemoveHrComponent<T>() where T : HrComponent
        {
            RemoveHrComponent(typeof(T));
        }

        private void RemoveHrComponent(Type type)
        {
            LinkedListNode<HrComponent> current = m_components.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    m_components.Remove(current);
                    break;
                }

                current = current.Next;
            }
        }

        public T GetHrComponent<T>() where T : HrComponent
        {
            var component = (T)GetHrComponent(typeof(T));
            if (component == null)
            {
                component = AddHrComponent<T>();
                if (component == null)
                    HrLogger.Log(string.Format("can not find the component '{0}'", typeof(T).FullName));
            }
            return component;
        }

        public HrComponent GetHrComponent(Type type)
        {
            LinkedListNode<HrComponent> current = m_components.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    return current.Value;
                }

                current = current.Next;
            }

            return null;
        }
        #endregion

        #region Module
        public T GetModule<T>() where T : class
        {
            Type type = typeof(T);
            return GetModule(type) as T;
        }

        private HrModule GetModule(Type type)
        {
            foreach (HrModule module in m_modules)
            {
                if (module.GetType() == type)
                {
                    return module;
                }
            }

            return CreateModule(type);
        }

        private HrModule CreateModule(Type type)
        {
            HrModule module = (HrModule)Activator.CreateInstance(type);
            if (module == null)
            {
                HrLogger.LogError("HrGameWorld CreateModule Error! " + module.GetType().FullName);
            }

            LinkedListNode<HrModule> current = m_modules.First;
            while (current != null)
            {
                current = current.Next;
            }
            if (current != null)
            {
                m_modules.AddBefore(current, module);
            }
            else
            {
                m_modules.AddLast(module);
            }

            return module;
        }
        #endregion

        public void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {
            foreach (var module in m_modules)
            {
                module.OnUpdate(fElapseSeconds, fRealElapseSeconds);
            }
        }
    }

}
