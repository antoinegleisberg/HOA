using System;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building), typeof(Storage))]
    public class House : MonoBehaviour
    {
        [SerializeField] private List<Citizen> _residents;
        [SerializeField] private int _maxResidents;

        public bool IsFull => _residents.Count >= _maxResidents;

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
    }
}
