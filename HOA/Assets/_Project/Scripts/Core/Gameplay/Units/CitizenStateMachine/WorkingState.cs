using antoinegleisberg.StateMachine;
using System.Collections;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    public class WorkingState : BaseState<Citizen>
    {
        private Citizen _citizen;

        private BaseWorkBehaviour _workBehaviour;

        public override void EnterState(Citizen citizen)
        {
            _citizen = citizen;

            _workBehaviour = DetermineWorkBehaviour();

            citizen.StartCoroutine(WorkCoroutine());
        }

        public override void ExitState(Citizen citizen)
        {
            
        }

        public override void UpdateState(Citizen citizen)
        {

        }

        private IEnumerator WorkCoroutine()
        {
            yield return _citizen.MoveToBuilding(_citizen.Workplace.GetComponent<Building>());

            float startTime = Time.time;
            while (Time.time - startTime < _citizen.TimeAtWork)
            {
                yield return _citizen.StartCoroutine(_workBehaviour.ExecuteWork());
            }

            _citizen.SwitchState(_citizen.WanderingState);
        }

        private BaseWorkBehaviour DetermineWorkBehaviour()
        {
            Building workplaceBuilding = _citizen.Workplace.GetComponent<Building>();
            
            if (workplaceBuilding.IsProductionSite)
            {
                return new ProductionSiteWorkBehaviour(_citizen);
            }
            else if (workplaceBuilding.IsResourceGatheringSite)
            {
                return new ResourceGatheringSiteWorkBehaviour(_citizen);
            }
            else if (workplaceBuilding.IsConstructionSite)
            {
                return new ConstructionSiteWorkBehaviour(_citizen);
            }
            else if (workplaceBuilding.IsMainStorage)
            {
                return new MainStorageWorkBehaviour(_citizen);
            }
            else
            {
                throw new System.Exception("Workplace is not a valid workplace type !");
            }
        }
    }
}
