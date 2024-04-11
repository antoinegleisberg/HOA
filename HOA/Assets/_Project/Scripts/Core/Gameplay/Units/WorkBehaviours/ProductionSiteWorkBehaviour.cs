using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class ProductionSiteWorkBehaviour : BaseWorkBehaviour
    {
        private Storage WorkplaceStorage { get => _citizen.Workplace.GetComponent<Storage>(); }
        
        public ProductionSiteWorkBehaviour(Citizen citizen) : base(citizen)
        {
        }

        public override IEnumerator ExecuteWork()
        {
            ProductionSite productionSite = _citizen.Workplace.GetComponent<ProductionSite>();
            Recipe recipe = productionSite.GetRecipe();

            if (!WorkplaceStorage.ContainsItems(recipe.RequiredItems))
            {
                yield return _citizen.StartCoroutine(GetMaterialsCoroutine(recipe));
                yield break;
            }
            if (!WorkplaceStorage.CanAddItems(recipe.ProducedItems))
            {
                yield return _citizen.StartCoroutine(StoreItemsToMainStorage(new List<ScriptableItem>(recipe.ProducedItems.Keys)));
                yield break;
            }

            yield return _citizen.StartCoroutine(ProduceCoroutine(recipe));
        }

        private IEnumerator GetMaterialsCoroutine(Recipe recipe)
        {
            ScriptableItem missingItem = null;
            int missingAmount = 0;

            foreach (KeyValuePair<ScriptableItem, int> kvp in recipe.RequiredItems)
            {
                if (WorkplaceStorage.GetItemCount(kvp.Key) < kvp.Value)
                {
                    missingItem = kvp.Key;
                    missingAmount = kvp.Value - WorkplaceStorage.GetItemCount(kvp.Key);
                    break;
                }
            }

            if (missingItem == null)
            {
                Debug.LogError("No missing item found, but production is impossible");
                yield break;
            }

            ItemStorageInfo storageInfo = BuildingsDB.Instance.GetLocationOfResource(missingItem);

            Storage target = null;
            int availableAmount = 0;

            foreach (KeyValuePair<Storage, int> kvp in storageInfo.Availability)
            {
                Debug.LogWarning("Replace this by actual best storage");
                target = kvp.Key;
                availableAmount = kvp.Value;
                break;
            }

            if (target == null)
            {
                Debug.Log("No items available !");
                yield break;
            }

            yield return _citizen.StartCoroutine(_citizen.MoveToBuilding(target.GetComponent<Building>()));

            int actualAvailableAmount = target.GetItemCount(missingItem);
            int amountToGet = Mathf.Min(missingAmount, actualAvailableAmount);
            target.RemoveItems(missingItem, amountToGet);
            WorkplaceStorage.AddItems(missingItem, amountToGet);

            yield return _citizen.StartCoroutine(_citizen.MoveToBuilding(_citizen.Workplace.GetComponent<Building>()));
        }

        private IEnumerator StoreItemsToMainStorage(List<ScriptableItem> items)
        {
            ScriptableItem itemToStore = null;
            int amountToStore = 0;
            foreach (ScriptableItem item in items)
            {
                if (_citizen.Workplace.GetComponent<Storage>().GetItemCount(item) > 0)
                {
                    itemToStore = item;
                    amountToStore = WorkplaceStorage.GetItemCount(item);
                }
            }

            if (itemToStore == null)
            {
                Debug.LogError("No item to store away !");
                yield break;
            }

            ItemStorageInfo storageInfo = BuildingsDB.Instance.GetAvailableMainStorage(itemToStore, amountToStore);

            Storage target = null;
            int availableSpace = 0;
            
            foreach (KeyValuePair<Storage, int> kvp in storageInfo.Availability)
            {
                Debug.LogWarning("Replace this by actual best storage");
                target = kvp.Key;
                availableSpace = kvp.Value;
                break;
            }

            if (target == null)
            {
                Debug.Log("No main storage available !");
                yield break;
            }

            yield return _citizen.StartCoroutine(_citizen.MoveToBuilding(target.GetComponent<Building>()));

            int addedQuantity = target.AddAsManyAsPossible(itemToStore, amountToStore);
            WorkplaceStorage.RemoveItems(itemToStore, addedQuantity);

            yield return _citizen.StartCoroutine(_citizen.MoveToBuilding(_citizen.Workplace.GetComponent<Building>()));
        }

        private IEnumerator ProduceCoroutine(Recipe recipe)
        {
            WorkplaceStorage.RemoveItems(recipe.RequiredItems);
            WorkplaceStorage.AddItems(recipe.ProducedItems);
            yield return new WaitForSeconds(recipe.ProductionTime);

            foreach (KeyValuePair<ScriptableItem, int> producedItem in recipe.ProducedItems)
            {
                Debug.Log($"Produced {producedItem.Value} {producedItem.Key.Name}");
            }
        }
    }
}
