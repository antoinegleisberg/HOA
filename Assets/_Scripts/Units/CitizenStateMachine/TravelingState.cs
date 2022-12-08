using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelingState : CitizenBaseState
{
    private Destination _destination;
    private float _speed = 1;
    private Storage _storage;

    public void SetDestination(Destination destination) {_destination = destination;}
    
    public override void Init(Citizen citizen) { }

    public override void EnterState(Citizen citizen)
    {
        if (citizen.worksite == null && _destination == Destination.Worksite) citizen.SwitchState(citizen.searchWorksiteState);
    }

    public override void UpdateState(Citizen citizen)
    {
        Move(citizen);
    }

    public override void ExitState(Citizen citizen) { }

    public override void OnDestroy(Citizen citizen) { }

    private void Move(Citizen citizen)
    {
        Vector3 destinationPosition;
        CitizenBaseState nextState;
        switch (_destination)
        {
            case Destination.House:
                destinationPosition = citizen.house.transform.position;
                nextState = citizen.breakState;
                break;
            case Destination.Worksite:
                destinationPosition = citizen.worksite.transform.position;
                nextState = citizen.workState;
                break;
            case Destination.Storage:
                if (FindStorage()) {
                    destinationPosition = _storage.transform.position;
                    nextState = citizen.storeState;
                }
                else
                {
                    destinationPosition = citizen.house.transform.position;
                    nextState = citizen.breakState;
                }
                break;
            default:
                destinationPosition = citizen.house.transform.position;
                nextState = citizen.breakState;
                break;
        }
        bool foundDestination = (destinationPosition - citizen.transform.position).magnitude < 0.1f;
        if (foundDestination) citizen.SwitchState(nextState);
        else citizen.transform.position += (destinationPosition - citizen.transform.position).normalized * _speed * Time.deltaTime;
    }

    bool FindStorage()
    {
        List<Storage> storages = new List<Storage>();
        foreach (BaseBuilding building in GridManager.instance.GetBuildingsList())
        {
            Storage storage = building.GetComponent<Storage>();
            if (storage != null) storages.Add(storage);
        }
        if (storages.Count == 0) return false;
        _storage = storages[Random.Range(0, storages.Count)];
        return true;
    }
}

public enum Destination
{
    House,
    Worksite,
    Storage
}