using System.Collections;
using System.Collections.Generic;
using System.Linq;
using antoinegleisberg.Types;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [RequireComponent(typeof(Citizen))]
    public class CitizenItemTransport : MonoBehaviour
    {
        private Citizen _citizen => GetComponent<Citizen>();

        public IEnumerator StoreLimitingItemsToMainStorage(IEnumerable<ScriptableItem> items, Storage storageToTakeFrom)
        {
            if (!items.Any())
            {
                yield break;
            }

            // Finds the item that limits the capacity the most
            ScriptableItem itemToStore = items
                .MinBy(item => storageToTakeFrom.GetCapacityForItem(item, storageToTakeFrom.GetItemCount(item)));

            yield return StartCoroutine(StoreItemsToMainStorage(itemToStore, storageToTakeFrom.GetItemCount(itemToStore), storageToTakeFrom));
        }

        public IEnumerator GetItemsFromAvailableStorage(IReadOnlyDictionary<ScriptableItem, int> requiredItems, IReadOnlyDictionary<ScriptableItem, int> availableItems, Storage storageToTakeTo)
        {
            Dictionary<ScriptableItem, int> neededItems = requiredItems.Diff(availableItems);

            if (!neededItems.Any())
            {
                yield break;
            }
            
            ScriptableItem neededItem = null;
            int neededAmount = 0;
            Storage storageWithAvailableItem = null;

            foreach (KeyValuePair<ScriptableItem, int> kvp in neededItems)
            {
                neededItem = kvp.Key;
                neededAmount = kvp.Value;
                storageWithAvailableItem = GetOptimalStorageToGetItemsFromStorage(neededItem, neededAmount);

                if (storageWithAvailableItem != null)
                {
                    break;
                }
            }

            if (storageWithAvailableItem == null)
            {
                Debug.Log("None of the items are available anywhere !");
                yield break;
            }

            yield return StartCoroutine(_citizen.MoveToBuilding(storageWithAvailableItem.GetComponent<Building>()));

            if (storageWithAvailableItem == null || storageToTakeTo == null)
            {
                // If the buildings were destroyed since we last checked, cancel transport
                yield break;
            }

            int actualAvailableAmount = storageWithAvailableItem.GetItemCount(neededItem);
            int amountToGet = Mathf.Min(neededAmount, actualAvailableAmount);
            storageWithAvailableItem.RemoveItems(neededItem, amountToGet);
            _citizen.PickUpItem(neededItem, amountToGet);

            yield return StartCoroutine(_citizen.MoveToBuilding(storageToTakeTo.GetComponent<Building>()));

            if (storageToTakeTo == null)
            {
                // If the buildings were destroyed since we last checked, cancel transport
                yield break;
            }

            int addedAmount = storageToTakeTo.AddAsManyAsPossible(neededItem, amountToGet);
            _citizen.DepositItem(neededItem, addedAmount);
            
            if (_citizen.CarriesItems)
            {
                _citizen.SwitchState(_citizen.GetRidOfInventoryState);
            }
        }

        private IEnumerator StoreItemsToMainStorage(ScriptableItem itemToStore, int amount, Storage storageToTakeFrom)
        {
            Storage target = GetOptimalStorageToTakeItemsToStorage(itemToStore, amount, storageToTakeFrom);

            if (target == null)
            {
                yield break;
            }

            storageToTakeFrom.RemoveItems(itemToStore, amount);
            _citizen.PickUpItem(itemToStore, amount);
            
            yield return StartCoroutine(_citizen.MoveToBuilding(target.GetComponent<Building>()));

            if (target == null || storageToTakeFrom == null)
            {
                // If one of the storages was destroyed, cancel the transfer
                yield break;
            }

            int addedAmount = target.AddAsManyAsPossible(itemToStore, amount);
            _citizen.DepositItem(itemToStore, addedAmount);

            if (_citizen.CarriesItems)
            {
                _citizen.SwitchState(_citizen.GetRidOfInventoryState);
            }
        }
    
        private Storage GetOptimalStorageToGetItemsFromStorage(ScriptableItem neededItem, int neededAmount)
        {
            ItemStorageInfo storageInfo = BuildingsDB.Instance.GetLocationOfResource(neededItem);

            if (storageInfo.Availability.Count == 0)
            {
                return null;
            }

            // Phase 1: consider only storages that hold enough items, and take the closest one
            IEnumerable<Storage> potentialStorages = storageInfo.Availability
                .Where(kvp => (kvp.Key == neededItem && kvp.Value >= neededAmount))
                .Select(kvp => kvp.Key);
            if (potentialStorages.Count() >= 1)
            {
                Vector3 closestStoragePosition = potentialStorages
                    .Select(storage => storage.transform.position)
                    .ClosestTo(_citizen.Workplace.transform.position);
                Storage bestStorage = potentialStorages
                    .Where(storage => storage.transform.position == closestStoragePosition)
                    .First();
                return bestStorage;
            }

            // Phase 2: take the storage with the largest amount of the searched item
            Storage bestAlternateStorage = storageInfo.Availability
                .Select(kvp => kvp.Key)
                .MaxBy(storage => storageInfo.Availability[storage]);

            return bestAlternateStorage;
        }
    
        private Storage GetOptimalStorageToTakeItemsToStorage(ScriptableItem itemToStore, int amount, Storage storageToTakeFrom)
        {
            ItemStorageInfo storageInfo = BuildingsDB.Instance.GetAvailableMainStorage(itemToStore, amount);

            if (storageInfo.Availability.Count() == 0)
            {
                return null;
            }

            // Phase 1: consider only storages that have capacity for all items
            IEnumerable<Storage> potentialStorages = storageInfo.Availability
                .Where(kvp => (kvp.Value >= amount))
                .Select(kvp => kvp.Key);

            if (potentialStorages.Count() >= 1)
            {
                Vector3 closestStoragePosition = potentialStorages
                    .Select(storage => storage.transform.position)
                    .ClosestTo(storageToTakeFrom.transform.position);
                Storage bestStorage = potentialStorages
                    .Where(storage => storage.transform.position == closestStoragePosition)
                    .First();
                return bestStorage;
            }

            // Phase 2: take the storage with the largest amount of the searched item
            Storage bestAlternateStorage = storageInfo.Availability
                .Select(kvp => kvp.Key)
                .MaxBy(storage => storageInfo.Availability[storage]);

            return bestAlternateStorage;
        }
    }
}
