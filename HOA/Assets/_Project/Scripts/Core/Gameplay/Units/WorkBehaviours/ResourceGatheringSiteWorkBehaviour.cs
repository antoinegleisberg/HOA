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
                Debug.Log("Didn't find a resource site to harvest !");
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
            _citizen.PickUpItems(harvestedResources);
            yield return new WaitForSeconds(resourceSite.ScriptableResourceSite.HarvestTime);
            
            foreach (ScriptableItem sItem in harvestedResources.Keys)
            {
                Debug.Log($"Harvested {harvestedResources[sItem]} {sItem}");
            }

            yield return _citizen.StartCoroutine(_citizen.MoveToBuilding(_citizen.Workplace.GetComponent<Building>()));
            
            IReadOnlyDictionary<ScriptableItem, int> addedItems = _workplaceStorage.AddAsManyAsPossible(_citizen.CarriedItems);
            _citizen.DepositItems(addedItems);
            
            if (_citizen.CarriesItems)
            {
                _citizen.SwitchState(_citizen.GetRidOfInventoryState);
            }
        }

        private ResourceSite GetClosestResourceSite()
        {
            ResourceSiteType searchedType = _citizen.Workplace.GetComponent<ResourceGatheringSite>().ResourceSiteType;
            
            // Maybe this can be done more efficiently in case the map is huge with lots of resource sites
            // => use a quadtree instead ??
            List<ResourceSite> viableResourceSites = 
                MapManager.Instance.ResourceSites.Select((KeyValuePair<Vector3Int, ResourceSite> kvp) => kvp.Value)
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
