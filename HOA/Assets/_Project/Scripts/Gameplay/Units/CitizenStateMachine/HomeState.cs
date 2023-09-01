using antoinegleisberg.StateMachine;


namespace antoinegleisberg.HOA
{
    public class HomeState : BaseState<Citizen>
    {
        public override void EnterState(Citizen citizen)
        {
            if (citizen.Workplace == null)
            {
                citizen.SwitchState(citizen.SearchWorksiteState);
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
