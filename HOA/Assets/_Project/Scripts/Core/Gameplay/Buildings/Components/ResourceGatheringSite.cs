using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [RequireComponent(typeof(Building), typeof(Storage), typeof(Workplace))]
    public class ResourceGatheringSite : MonoBehaviour
    {
        [SerializeField] private ScriptableResourceGatheringSite _scriptableResourceGatheringSite;

        public ResourceSiteType ResourceSiteType => _scriptableResourceGatheringSite.ResourceSiteType;
    }
}
