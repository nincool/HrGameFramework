using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hr
{
    public class HrGameApp : MonoBehaviour 
    {
        /// <summary>
        /// 游戏内置版本号
        /// </summary>
        [SerializeField]
        private string m_strGameVersion = string.Empty;

        [SerializeField]
        private bool m_bNeverSleep = true;

        [SerializeField]
        private bool m_bRunInBackGround = true;

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
                return m_bRunInBackGround;
            }
            set
            {
                Application.runInBackground = m_bRunInBackGround = value;
            }
        }

        /// <summary>
        /// 初始化场景
        /// </summary>
        public string EntryScene
        {
            get
            {
                return m_strEntryScene;
            }
            set
            {
                m_strEntryScene = value;
            }
        }

        protected virtual void Awake()
        {
            InitGameComponent();
        }

        protected virtual void Start()
        {
            HrLogger.Log("HrGameApp Start!");

            DontDestroyOnLoad(this);

            Application.runInBackground = RunInBackground;

            if (!string.IsNullOrEmpty(m_strEntryScene))
            {
                HrGameWorld.Instance.StartGame(m_strEntryScene);
            }
            else
            {
                HrLogger.LogError("HrGameApp Start EntryScene is null");
            }
        }

        private void InitGameComponent()
        {
            AddGameComponent<HrEventComponent>();
            AddGameComponent<HrFSMComponent>();
            AddGameComponent<HrSceneComponent>();

            HrGameWorld.Instance.Init();
        }

        private void AddGameComponent<T>() where T : Component
        {
            GameObject eventComponent = new GameObject(typeof(T).FullName);
            eventComponent.AddComponent<T>();
            eventComponent.transform.parent = this.transform;
        }

        protected virtual void Update()
        {
            HrGameWorld.Instance.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

    }

}

