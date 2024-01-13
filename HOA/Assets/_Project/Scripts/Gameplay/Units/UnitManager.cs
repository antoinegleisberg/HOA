using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager Instance { get; private set; }

        [SerializeField] private Transform _citizensContainer;
        [SerializeField] private Citizen _citizenPrefab;

        private List<Citizen> _citizens;

        private void Awake()
        {
            Instance = this;
            _citizens = new List<Citizen>();
        }
        
        public Citizen SpawnCitizen(Vector3 position, House house)
        {
            Citizen citizen = Instantiate(_citizenPrefab, position, Quaternion.identity, _citizensContainer);
            citizen.SetHouse(house);
            _citizens.Add(citizen);
            return citizen;
        }
    }
}
