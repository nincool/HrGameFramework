using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.ObjectPool
{
    public class HrObjectWrapper
    {
        /// <summary>
        /// 是否拥有引用对象
        /// </summary>
        public bool ObtainObject
        {
            get;
            private set;
        }

        /// <summary>
        /// 实际对象
        /// </summary>
        private object m_WrapObject;

        
        public object GiveupRef()
        {
            if (!ObtainObject)
            {
                throw new HrException(string.Format("the objectwrapper does not has a obj"));
            }
            object objRet = m_WrapObject;
            m_WrapObject = null;
            ObtainObject = false;

            return objRet;
        }

        public void ObtainRef(object obj)
        {
            if (ObtainObject)
            {
                throw new HrException(string.Format("the objectwrapper has a obj {0}", ObtainObject.GetType().FullName));
            }
            ObtainObject = true;

            m_WrapObject = obj;
        }
    }
}



