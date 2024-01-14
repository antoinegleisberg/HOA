using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building))]
    public class House : MonoBehaviour
    {
        [SerializeField] private List<Citizen> _residents;

        private void Awake()
        {
            Citizen resident = UnitManager.Instance.SpawnCitizen(transform.position, this);
            _residents = new List<Citizen>
            {
                resident
            };
        }
    }
}
