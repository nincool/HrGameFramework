using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Input
{
    public interface IInputManager
    {

        void AddAlias(string strName, int nID);

        void AddAlias(string strName, int nPrimaryID, int nSupportID);

        bool IsPressed(int nButtonID);

        bool IsPressed(string strAlias);

        float MouseX();

        float MouseY();

        float MouseXDelta();

        float MouseYDelta();

        float MouseAxisX();

        float MouseAxisY();

        float MouseWheel();

        bool IsJustPressed(string strAlias);

        bool IsDoublePressed(string strAlias);

        bool IsJustReleased(string strAlias);

        bool IsJustSingleReleased(string strAlias);

        bool IsJustDoubleReleased(string strAlias);
    }
}
