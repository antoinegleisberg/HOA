using antoinegleisberg.StateMachine;
using System.Collections;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class WanderingState : BaseState<Citizen>
    {
        public override void EnterState(Citizen citizen)
        {
            citizen.StartCoroutine(Wander(citizen));
        }

        public override void ExitState(Citizen citizen)
        {
            
        }

        public override void UpdateState(Citizen citizen)
        {
            
        }

        private IEnumerator Wander(Citizen citizen)
        {
            float startTime = Time.time;

            while (Time.time - startTime < citizen.TimeWandering)
            {
                Vector3 targetPosition = GridManager.Instance.GetRandomWalkablePosition(citizen.transform.position, citizen.WanderingDistance);
                yield return citizen.StartCoroutine(citizen.MoveToPosition(targetPosition));

                yield return new WaitForSeconds(1f);
            }

            citizen.SwitchState(citizen.HomeState);
        }
    }
}
