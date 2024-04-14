using UnityEngine;

namespace antoinegleisberg.HOA
{
    [CreateAssetMenu(fileName = "NewResourceGatheringSite", menuName = "ScriptableObjects/Buildings/Resource Gathering Site")]
    public class ScriptableResourceGatheringSite : ScriptableObject
    {
        public ResourceSiteType ResourceSiteType;
    }
}
