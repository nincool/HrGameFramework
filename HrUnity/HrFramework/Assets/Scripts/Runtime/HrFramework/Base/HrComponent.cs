using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr
{
    public class HrComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            HrGameWorld.Instance.AddHrComponent(this);
        }

        protected virtual void Start()
        {

        }

        public virtual void Init()
        {

        }
    }
}
