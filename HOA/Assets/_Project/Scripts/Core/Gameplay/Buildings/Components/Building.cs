using antoinegleisberg.Saving;
using UnityEngine;

namespace antoinegleisberg.HOA
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
        public bool IsHouse => GetComponent<House>() != null;
        public bool IsProductionSite => GetComponent<ProductionSite>() != null;
        public bool IsResourceGatheringSite => GetComponent<ResourceGatheringSite>() != null;
        public bool IsStorage => GetComponent<Storage>() != null;
        public bool IsWorkplace => GetComponent<Workplace>() != null;
        public bool IsMainStorage => IsStorage && !IsHouse && !IsProductionSite && !IsResourceGatheringSite;
    }
}
