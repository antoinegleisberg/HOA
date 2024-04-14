using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building), typeof(Storage))]
    public class ResourceGatheringSite : MonoBehaviour
    {
        [SerializeField] private ScriptableResourceGatheringSite _scriptableResourceGatheringSite;

        public ResourceSiteType ResourceSiteType {
            get
            {
                return _scriptableResourceGatheringSite.ResourceSiteType;
            }
        }
    }
}
