using antoinegleisberg.Saving;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [RequireComponent(typeof(GuidHolder))]
    public class Building : MonoBehaviour
    {
        [field: SerializeField] public ScriptableBuilding ScriptableBuilding { get; private set; }

        private void Awake()
        {
            if (ScriptableBuilding != null && !IsConstructionSite)
            {
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                sr.sprite = ScriptableBuilding.Sprite;
            }
        }

        public void Initialize(ScriptableBuilding scriptableBuilding)
        {
            ScriptableBuilding = scriptableBuilding;
        }

        public bool IsConstructionSite => GetComponent<ConstructionSite>() != null;
        public ConstructionSite ConstructionSite => GetComponent<ConstructionSite>();
        public bool IsHouse => GetComponent<House>() != null;
        public House House => GetComponent<House>();
        public bool IsProductionSite => GetComponent<ProductionSite>() != null;
        public ProductionSite ProductionSite => GetComponent<ProductionSite>();
        public bool IsResourceGatheringSite => GetComponent<ResourceGatheringSite>() != null;
        public ResourceGatheringSite ResourceGatheringSite => GetComponent<ResourceGatheringSite>();
        public bool IsStorage => GetComponent<Storage>() != null;
        public Storage Storage => GetComponent<Storage>();
        public bool IsWorkplace => GetComponent<Workplace>() != null;
        public Workplace Workplace => GetComponent<Workplace>();
        public bool IsMainStorage => IsStorage && !IsHouse && !IsProductionSite && !IsResourceGatheringSite;
    }
}
