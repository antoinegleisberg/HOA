using System.Collections;
using System.Collections.Generic;
using System.Linq;
using antoinegleisberg.Types;
using UnityEngine;

namespace antoinegleisberg.HOA
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
            ScriptableItem itemToStore = null;
            int minRemainingCapacity = int.MaxValue;
            foreach (ScriptableItem item in items)
            {
                if (storageToTakeFrom.GetCapacityForItem(item, storageToTakeFrom.GetItemCount(item)) < minRemainingCapacity)
                {
                    itemToStore = item;
                    minRemainingCapacity = storageToTakeFrom.GetCapacityForItem(item, storageToTakeFrom.GetItemCount(item));
                }
            }

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

            ItemStorageInfo storageInfo = BuildingsDB.Instance.GetLocationOfResource(neededItem);

            Storage storageWithAvailableItem = null;
            int availableAmount = 0;

            foreach (KeyValuePair<Storage, int> kvp in storageInfo.Availability)
            {
                Debug.LogWarning("ToDo: Replace this by actual best/closest storage");
                storageWithAvailableItem = kvp.Key;
                availableAmount = kvp.Value;
                break;
            }

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
            ItemStorageInfo storageInfo = BuildingsDB.Instance.GetAvailableMainStorage(itemToStore, amount);

            Storage target = null;
            int capacity;
            foreach (KeyValuePair<Storage, int> kvp in storageInfo.Availability)
            {
                Debug.LogWarning("ToDo: choose the best possible/closest storage instead");
                target = kvp.Key;
                capacity = kvp.Value;
                break;
            }

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
    }
}
