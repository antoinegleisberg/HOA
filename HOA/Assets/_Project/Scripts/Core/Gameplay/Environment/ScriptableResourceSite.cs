using antoinegleisberg.Types;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName = "Resource Site", menuName = "ScriptableObjects/Resource Site")]
    public class ScriptableResourceSite : ScriptableObject
    {
        [SerializeField] private List<Pair<ScriptableItem, int>> _availableItemsPerHarvest;
        public IReadOnlyDictionary<ScriptableItem, int> AvailableItemsPerHarvest => _availableItemsPerHarvest.ToDictionary();

        [field: SerializeField] private List<HarvestStage> _harvestStageInfos;
        public IReadOnlyList<HarvestStage> HarvestStageInfos => _harvestStageInfos;

        [field: SerializeField] public bool Replenishes { get; private set; }
        [field: SerializeField] private List<GrowthStage> _growthStages;
        public IReadOnlyList<GrowthStage> GrowthStages => _growthStages;

        [field: SerializeField] public ResourceSiteType ResourceSiteType {get; private set; }
        [field: SerializeField] public float HarvestTime { get; private set; }
    }

    [Serializable]
    public struct HarvestStage
    {
        public int NumberOfHarvests;
        public Sprite Sprite;
    }

    [Serializable]
    public struct GrowthStage
    {
        public float GrowthTime;
        public Sprite Sprite;
    }
}
