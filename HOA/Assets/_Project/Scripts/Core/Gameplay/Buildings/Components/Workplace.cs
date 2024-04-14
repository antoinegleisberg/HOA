using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building))]
    public class Workplace : MonoBehaviour
    {
        [SerializeField] private ScriptableWorkplace _scriptableWorkplace;
        [SerializeField] private List<Citizen> _workers;

        private int _maxWorkers {
            get
            {
                return _scriptableWorkplace.MaxWorkers;
            }
        }

        private void Awake()
        {
            _workers = new List<Citizen>(_maxWorkers);
        }

        public void AddWorker(Citizen worker)
        {
            if (RemainingWorkersSpace() <= 0)
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
