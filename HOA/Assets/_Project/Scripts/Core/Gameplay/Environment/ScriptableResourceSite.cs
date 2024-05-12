using antoinegleisberg.Types;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName = "Resource Site", menuName = "ScriptableObjects/Resource Site")]
    public class ScriptableResourceSite : ScriptableObject
    {
        [field:SerializeField] public List<Pair<ScriptableItem, int>> AvailableItemsPerHarvest {get; private set; }
        [field:SerializeField] public int MaxHarvests {get; private set; }
        [field:SerializeField] public ResourceSiteType ResourceSiteType {get; private set; }
        [field: SerializeField] public float HarvestTime { get; private set; }
    }
}
