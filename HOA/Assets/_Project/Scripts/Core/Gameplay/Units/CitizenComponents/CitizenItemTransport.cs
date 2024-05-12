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

            KeyValuePair<ScriptableItem, int> neededItemKvp = neededItems.First();
            ScriptableItem neededItem = neededItemKvp.Key;
            int neededAmount = neededItemKvp.Value;

            Storage storageWithAvailableItem = GetOptimalStorageToGetItemsFromStorage(neededItem, neededAmount);

            if (storageWithAvailableItem == null)
            {
                Debug.Log("No items available !");
                yield break;
            }

            yield return StartCoroutine(_citizen.MoveToBuilding(storageWithAvailableItem.GetComponent<Building>()));

            int actualAvailableAmount = storageWithAvailableItem.GetItemCount(neededItem);
            int amountToGet = Mathf.Min(neededAmount, actualAvailableAmount);
            storageWithAvailableItem.RemoveItems(neededItem, amountToGet);
            Debug.LogWarning("This could not be possible in case other items were added in the meantime: add citizen inventory");
            storageToTakeTo.AddItems(neededItem, amountToGet);

            yield return StartCoroutine(_citizen.MoveToBuilding(storageToTakeTo.GetComponent<Building>()));
        }

        private IEnumerator StoreItemsToMainStorage(ScriptableItem itemToStore, int amount, Storage storageToTakeFrom)
        {
            Storage target = GetOptimalStorageToTakeItemsToStorage(itemToStore, amount, storageToTakeFrom);

            if (target == null)
            {
                yield break;
            }

            storageToTakeFrom.RemoveItems(itemToStore, amount);
            yield return StartCoroutine(_citizen.MoveToBuilding(target.GetComponent<Building>()));
            int addedAmount = target.AddAsManyAsPossible(itemToStore, amount);
            Debug.LogWarning("I believe this could fail if in the meantime, another worker produced items. Change this to add items to citizen inventory instead - also creates more realistic transport");
            storageToTakeFrom.AddItems(itemToStore, amount - addedAmount);
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
                .Where(kvp => (kvp.Key == neededItem))
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
                .Where(kvp => (kvp.Key == itemToStore && kvp.Value >= amount))
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
                .Where(kvp => (kvp.Key == itemToStore))
                .Select(kvp => kvp.Key)
                .MaxBy(storage => storageInfo.Availability[storage]);

            return bestAlternateStorage;
        }
    }
}
