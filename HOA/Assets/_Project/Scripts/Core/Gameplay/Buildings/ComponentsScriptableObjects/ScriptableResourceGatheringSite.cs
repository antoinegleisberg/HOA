using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName = "NewResourceGatheringSite", menuName = "ScriptableObjects/Buildings/Resource Gathering Site")]
    public class ScriptableResourceGatheringSite : ScriptableObject
    {
        [field: SerializeField] public ResourceSiteType ResourceSiteType { get; private set; }
    }
}
