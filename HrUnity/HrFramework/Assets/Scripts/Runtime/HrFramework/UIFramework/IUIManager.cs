using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.UI
{
    public interface IUIManager
    {
        /// <summary>
        /// 把UIRoot和场景中的UIRoot关联
        /// </summary>
        void AttachUIRoot();

        /// <summary>
        /// UI Awake的时候主动注册到UIManager里
        /// </summary>
        void RegisterUIView(HrUIView uiView);


        void Clear();

    }
}
