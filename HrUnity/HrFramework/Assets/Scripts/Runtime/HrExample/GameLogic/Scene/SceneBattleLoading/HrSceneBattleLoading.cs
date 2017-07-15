using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Scene
{
    public class HrSceneBattleLoading : HrScene
    {

        public HrSceneBattleLoading(HrSceneManager sceneManager) :base(sceneManager)
        {
        }

        protected override void AddProcedure()
        {
            m_fsmProcedureStateMachine.AddState(new Procedure.HrSceneBattleLoading.HrProcedurePreload(this));
        }


    }
}
