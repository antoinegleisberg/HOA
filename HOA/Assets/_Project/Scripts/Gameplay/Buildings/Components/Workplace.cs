using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building))]
    public class Workplace : MonoBehaviour
    {
        [SerializeField] private int _maxWorkers;
        [SerializeField] private List<Citizen> _workers;

        private void Awake()
        {
            _workers = new List<Citizen>(_maxWorkers);
        }

        public void AddWorker(Citizen worker)
        {
            if (_workers.Count >= _maxWorkers - 1)
            {
                throw new System.Exception("Workplace is full");
            }
            _workers.Add(worker);
        }

        public int RemainingWorkersSpace()
        {
            return _maxWorkers - _workers.Count;
        }
    }
}
