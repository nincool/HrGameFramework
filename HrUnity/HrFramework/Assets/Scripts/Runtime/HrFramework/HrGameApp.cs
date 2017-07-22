using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Hr.Environment;

namespace Hr
{
    public class HrGameApp : HrUnitySingleton<HrGameApp>
    {
        /// <summary>
        /// 游戏内置版本号
        /// </summary>
        [SerializeField]
        private string m_strGameVersion = string.Empty;

        [SerializeField]
        private bool m_bNeverSleep = true;

        [SerializeField]
        private bool m_bRunInBackground = true;

        [SerializeField]
        private string m_strLaunch = string.Empty;

        [SerializeField]
        private string m_strEntryScene = string.Empty;

       
        public string GameVersion
        {
            get
            {
                return m_strGameVersion;
            }
            set
            {
                m_strGameVersion = value;
            }
        }


        /// <summary>
        /// 获取或设置是否禁止休眠。
        /// </summary>
        public bool NeverSleep
        {
            get
            {
                return m_bNeverSleep;
            }
            set
            {
                m_bNeverSleep = value;
                Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
        }


        /// <summary>
        /// 获取或设置是否允许后台运行。
        /// </summary>
        public bool RunInBackground
        {
            get
            {
                return m_bRunInBackground;
            }
            set
            {
                Application.runInBackground = m_bRunInBackground = value;
            }
        }

        /// <summary>
        /// 启动接口
        /// </summary>
        public string Launch
        {
            get
            {
                return m_strLaunch;
            }
        }

        /// <summary>
        /// 初始化Scene
        /// </summary>
        public string EntranceScene
        {
            get
            {
                return m_strEntryScene;
            }
        }

        #region Component Selected

        [SerializeField]
        private string m_strDataTableModule;
        public string DataTableModule
        {
            get
            {
                return m_strDataTableModule;
            }
        }

        [SerializeField]
        private string m_strEventModule;
        public string EventModule
        {
            get
            {
                return m_strEventModule;
            }
        }

        [SerializeField]
        private string m_strFSMModule;
        public string FSMModule
        {
            get
            {
                return m_strFSMModule;
            }
        }

        [SerializeField]
        private string m_strReleasePoolModule;
        public string ReleasePoolModule
        {
            get
            {
                return m_strReleasePoolModule;
            }
        }

        [SerializeField]
        private string m_strResourceModule;
        public string ResourceModule
        {
            get
            {
                return m_strResourceModule;
            }
        }

        [SerializeField]
        private string m_strSceneModule;
        public string SceneModule
        {
            get
            {
                return m_strSceneModule;
            }
        }

        [SerializeField]
        private string m_strUIModule;
        public string UIModule
        {
            get
            {
                return m_strUIModule;
            }
        }
        
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        protected void Awake()
        {
            if (HrEnvironment.IsEditorMode)
            {
                HrLogger.Log("GameApp will use editor model!");
            }
            Initialize();
        }

        protected  void Start()
        {
            HrLogger.Log("HrGameApp Start!");

            Application.runInBackground = RunInBackground;

            HrGameWorld.Instance.StartGame();
        }

        private void Initialize()
        {
            //预初始化一些模块
            PreInitializeGameModule(DataTableModule);
            PreInitializeGameModule(EventModule);
            PreInitializeGameModule(FSMModule);
            PreInitializeGameModule(ReleasePoolModule);
            PreInitializeGameModule(ResourceModule);
            PreInitializeGameModule(SceneModule);
            PreInitializeGameModule(UIModule);


            HrGameWorld.Instance.EntryScene = EntranceScene;
            HrGameWorld.Instance.ComponentRoot = this.transform;

            HrGameWorld.Instance.Initialize(m_strLaunch);
        }

        private void PreInitializeGameModule(string strModuleType)
        {
            if (!string.IsNullOrEmpty(strModuleType))
            {
                Type t = Type.GetType(strModuleType);
                if (t != null)
                {
                    HrGameWorld.Instance.GetModule(t);
                }
            }
        }

        protected virtual void Update()
        {
            HrGameWorld.Instance.OnUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        }
    }

}

