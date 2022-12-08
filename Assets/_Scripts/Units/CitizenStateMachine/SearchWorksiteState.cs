using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchWorksiteState : CitizenBaseState
{
    private List<Worksite> _worksites;
    
    public override void Init(Citizen citizen)
    {
        GameEvents.instance.onExitPreviewBuildingGameState += FindAllWorksites;
    }

    public override void EnterState(Citizen citizen)
    {
        FindAllWorksites();
    }

    public override void UpdateState(Citizen citizen)
    {
        CheckAvailableWorksites(citizen);
    }

    public override void ExitState(Citizen citizen) { }

    public override void OnDestroy(Citizen citizen)
    {
        GameEvents.instance.onExitPreviewBuildingGameState -= FindAllWorksites;
    }

    private void FindAllWorksites()
    {
        _worksites = new List<Worksite>();
        List<BaseBuilding> buildings = GridManager.instance.GetBuildingsList();
        foreach (BaseBuilding building in buildings)
        {
            Worksite worksite = building.GetComponent<Worksite>();
            if (worksite != null)
            {
                _worksites.Add(worksite);
            }
        }
    }

    private void CheckAvailableWorksites(Citizen citizen)
    {
        foreach (Worksite worksite in _worksites)
        {
            if (worksite.isAvailable)
            {
                citizen.worksite = worksite;
                worksite.AddWorker(citizen);
                citizen.travelState.SetDestination(Destination.Worksite);
                citizen.SwitchState(citizen.travelState);
                return;
            }
        }
    }
}
