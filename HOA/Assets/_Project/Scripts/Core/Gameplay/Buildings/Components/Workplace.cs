using System;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [RequireComponent(typeof(Building))]
    public class Workplace : MonoBehaviour
    {
        [SerializeField] private ScriptableWorkplace _scriptableWorkplace;
        [SerializeField] private List<Citizen> _workers;

        public IReadOnlyList<Citizen> Workers => _workers;

        private int _maxWorkers => _scriptableWorkplace.MaxWorkers;

        private void Awake()
        {
            _workers = new List<Citizen>(_maxWorkers);
        }

        public void AddWorker(Citizen worker)
        {
            if (RemainingWorkersSpace() <= 0)
            {
                throw new Exception("Workplace is full");
            }
            _workers.Add(worker);
            if (TryGetComponent(out ProductionSite productionSite))
            {
                productionSite.SetRecipe(worker, productionSite.AvailableRecipes[0]);
            }
        }

        public void RemoveWorker(Citizen worker)
        {
            if (!_workers.Contains(worker))
            {
                throw new Exception("Citizen is not a worker of this workplace");
            }
            _workers.Remove(worker);
        }

        public int RemainingWorkersSpace()
        {
            return _maxWorkers - _workers.Count;
        }
    }
}
