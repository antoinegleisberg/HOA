using antoinegleisberg.SaveSystem;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(GuidHolder))]
    public class Building : MonoBehaviour
    {
        [field: SerializeField] public ScriptableBuilding ScriptableBuilding { get; private set; }

        public bool IsBuildSite => GetComponent<BuildSite>() != null;
        public bool IsHouse => GetComponent<House>() != null;
        public bool IsMainStorage => GetComponent<MainStorage>() != null;
        public bool IsProductionSite => GetComponent<ProductionSite>() != null;
        public bool IsResourceGatheringSite => GetComponent<ResourceGatheringSite>() != null;
        public bool IsStorage => GetComponent<Storage>() != null;
        public bool IsWorkplace => GetComponent<Workplace>() != null;
    }
}
