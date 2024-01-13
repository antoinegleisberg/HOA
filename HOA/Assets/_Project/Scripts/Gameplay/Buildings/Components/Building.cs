using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class Building : MonoBehaviour
    {
        [field: SerializeField] public ScriptableBuilding ScriptableBuilding { get; private set; }

        public bool IsHouse => GetComponent<House>() != null;
        public bool IsProductionSite => GetComponent<ProductionSite>() != null;
        public bool IsStorage => GetComponent<Storage>() != null;
        public bool IsWorkplace => GetComponent<Workplace>() != null;

        public bool IsMainStorage => IsStorage && IsWorkplace && !IsProductionSite;
    }
}
