using antoinegleisberg.StateMachine;
using System.Collections.Generic;

namespace antoinegleisberg.HOA.Core
{
    public class SearchWorksiteState : BaseState<Citizen>
    {
        public override void EnterState(Citizen citizen)
        {
            // This automatically gives the citizen a workplace if there is one available
            // ToDo: Potentially create a setting in which we allow to choose the default behaviour:
            // - Automatically assign a workplace
            // - Always wait for the player to assign a workplace
            // - Only automatically assign a workplace when finishing construction of a building, builder becomes worker
            List<Workplace> workplaces = BuildingsDB.Instance.GetAvailableWorkplaces();
            Workplace workplace = null;
            if (workplaces.Count > 0)
            {
                workplace = workplaces[0];
            }
            if (workplace != null)
            {
                citizen.ClaimWorkplace(workplace);
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
