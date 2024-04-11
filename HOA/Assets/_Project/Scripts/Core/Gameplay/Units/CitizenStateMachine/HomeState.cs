using antoinegleisberg.StateMachine;
using System.Collections;
using UnityEngine;


namespace antoinegleisberg.HOA
{
    public class HomeState : BaseState<Citizen>
    {
        public override void EnterState(Citizen citizen)
        {
            if (citizen.Home == null)
            {
                House house = BuildingsDB.Instance.GetAvailableHouse();
                if (house != null)
                {
                    citizen.ClaimHouse(house);
                }
            }
            
            if (citizen.Home != null)
            {
                citizen.StartCoroutine(HomeCoroutine(citizen));
            }
            else
            {
                citizen.SwitchState(citizen.WanderingState);
            }
        }

        public override void ExitState(Citizen citizen)
        {

        }

        public override void UpdateState(Citizen citizen)
        {

        }

        private IEnumerator HomeCoroutine(Citizen citizen)
        {
            yield return citizen.MoveToBuilding(citizen.Home.GetComponent<Building>());

            yield return new WaitForSeconds(citizen.TimeAtHome);

            if (citizen.Workplace == null)
            {
                citizen.SwitchState(citizen.SearchWorksiteState);
            }
            else
            {
                citizen.SwitchState(citizen.WorkingState);
            }
        }
    }
}
