using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building))]
    public class House : MonoBehaviour
    {
        private void Awake()
        {
            UnitManager.Instance.SpawnCitizen(transform.position, this);
        }
    }
}
