using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building))]
    public class House : MonoBehaviour
    {
        [field:SerializeField] public List<Citizen> Residents { get; private set; }

        private void Awake()
        {
            Residents = new List<Citizen>();
        }
    }
}
