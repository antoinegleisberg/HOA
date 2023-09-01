using antoinegleisberg.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class WorkingState : BaseState<Citizen>
    {
        public override void EnterState(Citizen citizen)
        {
            citizen.StartCoroutine(WorkingCoroutine(citizen));
        }

        public override void ExitState(Citizen citizen)
        {

        }

        public override void UpdateState(Citizen citizen)
        {

        }

        private IEnumerator WorkingCoroutine(Citizen citizen)
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(1);
            }
            citizen.SwitchState(citizen.StoringState);
        }
    }
}
