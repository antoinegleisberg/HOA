using antoinegleisberg.StateMachine;

namespace antoinegleisberg.HOA
{
    public class SearchWorksiteState : BaseState<Citizen>
    {
        public override void EnterState(Citizen citizen)
        {
            Building workplaceBuilding = GridManager.Instance.GetBuildingWithComponentOfType<Workplace>();
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
