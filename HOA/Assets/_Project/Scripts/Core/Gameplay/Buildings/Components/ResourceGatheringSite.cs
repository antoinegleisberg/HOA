using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building), typeof(Storage))]
    public class ResourceGatheringSite : MonoBehaviour
    {
        [field: SerializeField] public ResourceSiteType ResourceSiteType { get; private set; }
    }
}
