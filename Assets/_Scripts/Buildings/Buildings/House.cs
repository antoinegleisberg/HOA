using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : BaseBuilding
{
    private List<Citizen> _citizens;
    [SerializeField] private int _nbCitizens;
    [SerializeField] private Citizen _citizenPrefab;
    [SerializeField] private Transform _citizenContainer;

    public override void Init()
    {
        _citizenContainer = GameObject.Find("Citizens").transform;
        InstantiateCitizens();
    }

    private void InstantiateCitizens()
    {
        _citizens = new List<Citizen>();
        for (int i = 0; i < _nbCitizens; i++)
        {
            Citizen citizen = Instantiate(_citizenPrefab, transform.position, Quaternion.identity, _citizenContainer);
            citizen.Init(this);
            citizen.name = "Citizen " + _citizenContainer.childCount;
            _citizens.Add(citizen);
        }
    }
}
