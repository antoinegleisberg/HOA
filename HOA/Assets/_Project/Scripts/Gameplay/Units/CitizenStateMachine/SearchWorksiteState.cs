using antoinegleisberg.StateMachine;

namespace antoinegleisberg.HOA
{
    public class SearchWorksiteState : BaseState<Citizen>
    {
        public override void EnterState(Citizen citizen)
        {
            // works for now, but need to implement limitation of number of workers
            Building workplaceBuilding = BuildingsDB.Instance.GetBuildingWithComponentOfType<Workplace>();
            if (workplaceBuilding != null)
            {
                Workplace workplace = workplaceBuilding.GetComponent<Workplace>();
                citizen.SetWorkplace(workplace);
                citizen.SwitchState(citizen.WorkingState);
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
    }
}
