using com.ootii.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Input
{
    public class HrInputEasyInput : HrModule, IInputManager
    {
        public HrInputEasyInput()
        {
        }

        public override void Init()
        {
            InputManager.UseGamepad = false;
            InputManager.Initialize();
        }

        public override void OnUpdate(float fElapseSeconds, float fRealElapseSeconds)
        {

        }
        public override void OnUpdateEndOfFrame(float fElapseSeconds, float fRealElapseSeconds)
        {
            InputManager.Update();
        }

        public override void Shutdown()
        {

        }

        public void AddAlias(string strName, int nID)
        {
            InputManager.AddAlias(strName, nID);
        }

        public void AddAlias(string strName, int nPrimaryID, int nSupportID)
        {
            InputManager.AddAlias(strName, nPrimaryID, nSupportID);
        }

        public bool IsPressed(int nButtonID)
        {
            return InputManager.IsPressed(nButtonID);
        }

        public bool IsPressed(string strAlias)
        {
            return InputManager.IsPressed(strAlias);
        }

        public float MouseX()
        {
            return InputManager.MouseX;
        }

        public float MouseY()
        {
            return InputManager.MouseY;
        }

        public float MouseXDelta()
        {
            return InputManager.MouseXDelta;
        }

        public float MouseYDelta()
        {
            return InputManager.MouseYDelta;
        }

        public float MouseAxisX()
        {
            return InputManager.MouseAxisX;
        }

        public float MouseAxisY()
        {
            return InputManager.MouseAxisY;
        }

        public float MouseWheel()
        {
            return InputManager.GetValue(EnumInput.MOUSE_WHEEL);
        }

        public bool IsJustPressed(string strAlias)
        {
            return InputManager.IsJustPressed(strAlias);
        }

        public bool IsDoublePressed(string strAlias)
        {
            return InputManager.IsDoublePressed(strAlias);
        }

        public  bool IsJustReleased(string strAlias)
        {
            return InputManager.IsJustReleased(strAlias);
        }

        public  bool IsJustSingleReleased(string strAlias)
        {
            return InputManager.IsJustSingleReleased(strAlias);
        }

        public  bool IsJustDoubleReleased(string strAlias)
        {
            return InputManager.IsJustDoubleReleased(strAlias);
        }


    }
}

