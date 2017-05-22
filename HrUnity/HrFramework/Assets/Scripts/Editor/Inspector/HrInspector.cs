using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Hr.Editor
{
    public class HrInspector : UnityEditor.Editor
    {
        private bool m_bIsCompiling = false;

        /// <summary>
        /// 绘制事件。
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (m_bIsCompiling && !EditorApplication.isCompiling)
            {
                m_bIsCompiling = false;
                OnCompileComplete();
            }
            else if (!m_bIsCompiling && EditorApplication.isCompiling)
            {
                m_bIsCompiling = true;
                OnCompileStart();
            }
        }

        /// <summary>
        /// 编译开始事件。
        /// </summary>
        protected virtual void OnCompileStart()
        {

        }

        /// <summary>
        /// 编译完成事件。
        /// </summary>
        protected virtual void OnCompileComplete()
        {

        }

    }

}
