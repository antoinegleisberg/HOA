using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoringState : CitizenBaseState
{
    public override void Init(Citizen citizen)
    {

    }

    public override void EnterState(Citizen citizen)
    { 
        citizen.travelState.SetDestination(Destination.Worksite);
        citizen.SwitchState(citizen.travelState);
    }

    public override void UpdateState(Citizen citizen)
    {

    }

    public override void ExitState(Citizen citizen)
    {

    }

    public override void OnDestroy(Citizen citizens)
    {

    }
}

