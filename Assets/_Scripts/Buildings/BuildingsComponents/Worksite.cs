using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worksite : MonoBehaviour
{
    [SerializeField] private int _maxWorkers;
    public bool isAvailable { get { return _workers.Count < _maxWorkers; } }
    private List<Citizen> _workers;

    private void Awake()
    {
        _workers = new List<Citizen>();
    }

    public void AddWorker(Citizen citizen)
    {
        _workers.Add(citizen);
    }
}