﻿using System.Collections;
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

        /// <summary>
        /// 初始化
        /// </summary>
        protected  void Awake()
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
            HrGameWorld.Instance.ComponentRoot = this.transform;
            HrGameWorld.Instance.Initialize(m_strLaunch);
        }

        protected virtual void Update()
        {
            HrGameWorld.Instance.OnUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        }
    }

}

