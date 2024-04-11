using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using antoinegleisberg.Types;

namespace antoinegleisberg.HOA
{
    public class ResourceGatheringSiteWorkBehaviour : BaseWorkBehaviour
    {
        public ResourceGatheringSiteWorkBehaviour(Citizen citizen) : base(citizen) {}

        public override IEnumerator ExecuteWork()
        {
            ResourceSite resourceSite = GetRandomResourceSite();
            if (resourceSite == null)
            {
                Debug.LogWarning("Didn't find a resource site to harvest !");
                yield return new WaitForSeconds(1.0f);
                yield break;
            }

            Dictionary<ScriptableItem, int> harvestableResources = resourceSite.ScriptableResourceSite.AvailableItemsPerHarvest.ToDictionary();
            Storage workplaceStorage = _citizen.Workplace.GetComponent<Storage>();
            if (workplaceStorage.CanAddItems(harvestableResources))
            {
                yield return _citizen.StartCoroutine(HarvestResourceSite(resourceSite));
                yield break;
            }
            
            yield return _citizen.StartCoroutine(StoreItemsToMainStorage());
        }

        private IEnumerator StoreItemsToMainStorage()
        {
            Pair<ScriptableItem, int> itemInfo = GetItemToStoreAway();
            ScriptableItem item = itemInfo.First;
            int amount = itemInfo.Second;
            ItemStorageInfo storageInfo = BuildingsDB.Instance.GetAvailableMainStorage(item, amount);

            Storage target = null;
            int capacity;
            foreach (KeyValuePair<Storage, int> kvp in storageInfo.Availability)
            {
                Debug.LogWarning("ToDo: choose the best possible storage instead");
                target = kvp.Key;
                capacity = kvp.Value;
                break;
            }
            
            if (target == null)
            {
                yield break;
            }

            _citizen.Workplace.GetComponent<Storage>().RemoveItems(item, amount);
            yield return _citizen.StartCoroutine(_citizen.MoveToBuilding(target.GetComponent<Building>()));
            int addedAmount = target.AddAsManyAsPossible(item, amount);
            _citizen.Workplace.GetComponent<Storage>().AddItems(item, amount - addedAmount);
        }

        private Pair<ScriptableItem, int> GetItemToStoreAway()
        {
            foreach (KeyValuePair<ScriptableItem, int> kvp in _citizen.Workplace.GetComponent<Storage>().Items())
            {
                if (kvp.Value > 0)
                {
                    return new Pair<ScriptableItem, int>(kvp.Key, kvp.Value);
                }
            }
            return new Pair<ScriptableItem, int>(null, 0);
        }

        private IEnumerator HarvestResourceSite(ResourceSite resourceSite)
        {

            yield return _citizen.StartCoroutine(_citizen.MoveToPosition(resourceSite.transform.position));

            Dictionary<ScriptableItem, int> harvestedResources = resourceSite.Harvest();
            yield return new WaitForSeconds(resourceSite.ScriptableResourceSite.HarvestTime);
            
            foreach (ScriptableItem sItem in harvestedResources.Keys)
            {
                Debug.Log($"Harvested {harvestedResources[sItem]} {sItem}");
            }

            yield return _citizen.StartCoroutine(_citizen.MoveToBuilding(_citizen.Workplace.GetComponent<Building>()));

            _citizen.Workplace.GetComponent<Storage>().AddItems(harvestedResources);
        }

        private ResourceSite GetRandomResourceSite()
        {
            ResourceSiteType searchedType = _citizen.Workplace.GetComponent<ResourceGatheringSite>().ResourceSiteType;
            foreach (ResourceSite resourceSite in Object.FindObjectsOfType<ResourceSite>())
            {
                if (resourceSite.ScriptableResourceSite.ResourceSiteType == searchedType && !resourceSite.IsDepleted)
                {
                    return resourceSite;
                }
            }
            return null;
        }
    }
}
