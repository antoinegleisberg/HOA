using antoinegleisberg.StateMachine;
using System.Collections.Generic;

namespace antoinegleisberg.HOA
{
    public class SearchWorksiteState : BaseState<Citizen>
    {
        public override void EnterState(Citizen citizen)
        {
            List<Workplace> workplaces = BuildingsDB.Instance.GetAvailableWorkplaces();
            Workplace workplace = null;
            if (workplaces.Count > 0)
            {
                workplace = workplaces[0];
            }
            if (workplace != null)
            {
                citizen.SetWorkplace(workplace);
                workplace.AddWorker(citizen);
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
