using System;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [RequireComponent(typeof(Building), typeof(Storage))]
    public class House : MonoBehaviour
    {
        [SerializeField] private List<Citizen> _residents;
        [SerializeField] private ScriptableHouse _scriptableHouse;

        public bool IsFull => _residents.Count >= _scriptableHouse.MaxResidents;
        public IReadOnlyList<Citizen> Residents => _residents;
        public int ResidentsCount => _residents.Count;

        private void Awake()
        {
            _residents = new List<Citizen>();
        }

        public void AddResident(Citizen citizen)
        {
            if (IsFull)
            {
                throw new Exception("House is full");
            }
            
            _residents.Add(citizen);
        }

        public void RemoveResident(Citizen citizen)
        {
            if (!_residents.Contains(citizen))
            {
                throw new Exception("Citizen is not a resident of this house");
            }

            _residents.Remove(citizen);
        }
    }
}
