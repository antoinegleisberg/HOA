using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using antoinegleisberg.Types;

namespace antoinegleisberg.HOA.Core
{
    public class ResourceGatheringSiteWorkBehaviour : BaseWorkBehaviour
    {
        private Storage _workplaceStorage { get => _citizen.Workplace.GetComponent<Storage>(); }

        public ResourceGatheringSiteWorkBehaviour(Citizen citizen) : base(citizen) {}

        public override IEnumerator ExecuteWork()
        {
            ResourceSite resourceSite = GetClosestResourceSite();
            if (resourceSite == null)
            {
                Debug.LogWarning("Didn't find a resource site to harvest !");
                yield return new WaitForSeconds(1.0f);
                yield break;
            }

            IReadOnlyDictionary<ScriptableItem, int> harvestableResources = resourceSite.ScriptableResourceSite.AvailableItemsPerHarvest;
            if (_workplaceStorage.CanAddItems(harvestableResources))
            {
                yield return _citizen.StartCoroutine(HarvestResourceSite(resourceSite));
                yield break;
            }
            
            yield return _citizen.StartCoroutine(StoreItemsToMainStorage());
        }

        private IEnumerator StoreItemsToMainStorage()
        {
            yield return _citizen.StartCoroutine(_citizen.StoreLimitingItemsToMainStorage(_workplaceStorage.Items().Keys, _workplaceStorage));
        }

        private IEnumerator HarvestResourceSite(ResourceSite resourceSite)
        {

            yield return _citizen.StartCoroutine(_citizen.MoveToPosition(resourceSite.transform.position));

            IReadOnlyDictionary<ScriptableItem, int> harvestedResources = resourceSite.Harvest();
            yield return new WaitForSeconds(resourceSite.ScriptableResourceSite.HarvestTime);
            
            foreach (ScriptableItem sItem in harvestedResources.Keys)
            {
                Debug.Log($"Harvested {harvestedResources[sItem]} {sItem}");
            }

            yield return _citizen.StartCoroutine(_citizen.MoveToBuilding(_citizen.Workplace.GetComponent<Building>()));

            Debug.LogWarning("I believe this could fail if in the meantime, another worker produced items. Change this to add items to citizen inventory instead - also creates more realistic transport");
            _workplaceStorage.AddItems(harvestedResources);
        }

        private ResourceSite GetClosestResourceSite()
        {
            ResourceSiteType searchedType = _citizen.Workplace.GetComponent<ResourceGatheringSite>().ResourceSiteType;
            Debug.LogWarning("ToDo: implement a better way to reference the list of all existing resource sites -> map generator ?");
            List<ResourceSite> viableResourceSites = Object.FindObjectsOfType<ResourceSite>()
                .Where((ResourceSite rs) => rs.ScriptableResourceSite.ResourceSiteType == searchedType && !rs.IsDepleted)
                .ToList();
            if (viableResourceSites.Count == 0)
            {
                return null;
            }

            Vector3 closestResourceSitePosition = viableResourceSites
                .Select((ResourceSite rs) => rs.transform.position)
                .ClosestTo(_citizen.Workplace.transform.position);

            return viableResourceSites
                .Where((ResourceSite rs) => Vector3.Distance(rs.transform.position, closestResourceSitePosition) < Mathf.Epsilon)
                .FirstOrDefault();
        }
    }
}
