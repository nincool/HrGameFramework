using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.CameraControl
{
    [Serializable]
    public class HrMouseLook
    {
        private Transform m_cameraTransform;

        private Quaternion m_cameraTargetRot;

        [SerializeField]
        private float m_fXSensitivity = 1f;
        [SerializeField]
        private float m_fYSensitivity = 1f;
        [SerializeField]
        public float m_fRotateSmoothSpeed = 5f;

        public Vector3 m_v3CameraOffset = new Vector3(0.5f, 0.9f, -2f);

        private Transform m_aimTransform;

        private float m_fPitch;
        private float m_fYaw;

        private Vector3 m_v3SmoothPositionVelocity;
        private float m_fSmoothTime = 0.1f;

        private float m_fTurnSpeed = 1.5f;

        public void Init(Transform cameraTransform, Transform aimTransform)
        {
            m_cameraTransform = cameraTransform;
            m_cameraTargetRot = cameraTransform.localRotation;
            m_aimTransform = aimTransform;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void UpdateLookRotation()
        {
            
            float fXDelta = HrGameWorld.Instance.InputComponent.MouseAxisX() * m_fXSensitivity;
            float fYDelta = HrGameWorld.Instance.InputComponent.MouseAxisY() * m_fYSensitivity;

            var v3TargetPosition = m_aimTransform.position;
            var v3Direction = v3TargetPosition - m_cameraTransform.position;
            var v3LookRotation = Quaternion.Slerp(m_cameraTransform.rotation, Quaternion.LookRotation(v3Direction), m_fRotateSmoothSpeed * Time.deltaTime).eulerAngles;

            m_fPitch += fYDelta * m_fTurnSpeed * -1;
            m_fYaw += fXDelta * m_fTurnSpeed;

            m_cameraTransform.rotation = Quaternion.Euler(m_fPitch, m_fYaw, 0);
        }

        public void UpdateMove()
        {
            var v3LookPoint = m_aimTransform.position + m_v3CameraOffset.x * m_cameraTransform.right + m_v3CameraOffset.y * m_aimTransform.up + m_v3CameraOffset.z * m_cameraTransform.forward;

            m_cameraTransform.position = Vector3.SmoothDamp(m_cameraTransform.position, v3LookPoint, ref m_v3SmoothPositionVelocity, m_fSmoothTime);
        }
    }
}
