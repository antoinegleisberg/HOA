using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakState : CitizenBaseState
{
    public override void Init(Citizen citizen) { }

    public override void EnterState(Citizen citizen) { }

    public override void UpdateState(Citizen citizen)
    {
        citizen.travelState.SetDestination(Destination.Worksite);
        citizen.SwitchState(citizen.travelState);
    }

    public override void ExitState(Citizen citizen) { }

    public override void OnDestroy(Citizen citizen) { }
}
