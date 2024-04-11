using antoinegleisberg.Types;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [CreateAssetMenu(fileName = "Resource Site", menuName = "ScriptableObjects/Resource Site")]
    public class ScriptableResourceSite : ScriptableObject
    {
        public List<Pair<ScriptableItem, int>> AvailableItemsPerHarvest;
        public int MaxHarvests;
        public ResourceSiteType ResourceSiteType;
        public float HarvestTime;
    }
}
