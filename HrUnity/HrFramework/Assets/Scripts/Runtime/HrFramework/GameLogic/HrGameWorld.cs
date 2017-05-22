using System;
using System.Collections;
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

        //事件管理器组件 
        public HrEventComponent EventComponent { get; private set; }

        //状态机组件
        public HrFSMComponent FSMComponent { get; private set; }

        //场景管理器组件
        public HrSceneComponent SceneComponent { get; private set; }


        public HrUIManager UIManager { get; private set; }

        public HrSceneManager SceneManager { get; private set; }

        public HrGameWorld()
        {
            UIManager = new HrUIManager();
            SceneManager = new HrSceneManager();    
        }

        public void Init()
        {
            EventComponent = GetHrComponent<HrEventComponent>();
            FSMComponent = GetHrComponent<HrFSMComponent>();
            SceneComponent = GetHrComponent<HrSceneComponent>();

            foreach (var module in m_modules)
            {
                module.Init();
            }
        }

        public void StartGame(string strEntryScene)
        {
            HrLogger.Log("HrGameWorld Start Game EntryScene:" + strEntryScene);
            SceneComponent.SwitchToScene(strEntryScene);
        }

        #region HrComponent
        public void AddHrComponent(HrComponent component)
        {
            if (component == null)
            {
                HrLogger.LogError("Component is invalid.");
                return;
            }

            Type type = component.GetType();
            LinkedListNode<HrComponent> current = m_components.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    HrLogger.LogError(string.Format("Game framework component type '{0}' is already exist.", type.FullName));
                    return;
                }

                current = current.Next;
            }

            m_components.AddLast(component);
        }

        public void RemoveHrComponent<T>() where T : HrComponent
        {
            RemoveHrComponent(typeof(T));
        }

        public void RemoveHrComponent(Type type)
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
            return (T)GetHrComponent(typeof(T));
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

        public void Update(float fElapseSeconds, float fRealElapseSeconds)
        {

        }
    }

}
