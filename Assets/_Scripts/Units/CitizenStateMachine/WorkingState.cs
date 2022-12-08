using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingState : CitizenBaseState
{
    public override void Init(Citizen citizen)
    {

    }

    public override void EnterState(Citizen citizen)
    {
        if (Random.Range(0.0f, 1.0f) < 0.5) citizen.travelState.SetDestination(Destination.House);
        else citizen.travelState.SetDestination(Destination.Storage);
        citizen.SwitchState(citizen.travelState);
    }

    public override void UpdateState(Citizen citizen)
    {

    }

    public override void ExitState(Citizen citizen)
    {

    }

    public override void OnDestroy(Citizen citizen)
    {

    }
}
