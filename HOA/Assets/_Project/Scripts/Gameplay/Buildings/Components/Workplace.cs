using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building))]
    public class Workplace : MonoBehaviour
    {
        private List<Citizen> _workers;

        private void Awake()
        {
            _workers = new List<Citizen>();
        }

        public void AddWorker(Citizen worker)
        {
            _workers.Add(worker);
        }
    }
}
