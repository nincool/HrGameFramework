using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Character
{
    public class HrCharacterController : MonoBehaviour
    {
        private float m_fHorizontalMovement;
        private float m_fForwardMovement;
        private Quaternion m_lookRotation;

        [SerializeField]
        private float m_fXSensitivity = 0.1f;
        [SerializeField]
        private float m_fYSensitivity = 0.1f;

  

        protected virtual void Awake()
        {

        }

        private void Update()
        {

        }

        protected virtual void FixedUpdate()
        {

            m_lookRotation = Camera.main.transform.rotation;

            UpdateMovement();
        }

        protected virtual void UpdateMovement()
        {
            float fXDelta = 0;
            float fYDelta = 0;
            if (HrGameWorld.Instance.InputComponent.IsPressed("Left"))
            {
                fXDelta += -m_fXSensitivity;
            }
            if (HrGameWorld.Instance.InputComponent.IsPressed("Forward"))
            {
                fYDelta += m_fYSensitivity;
            }


            this.transform.position += (this.transform.right * fXDelta + this.transform.forward * fYDelta); 

            this.transform.rotation = m_lookRotation;

        }

    }
}
