using antoinegleisberg.Inventory;
using antoinegleisberg.Types;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building))]
    public class Storage : MonoBehaviour, IInventory<ScriptableItem>
    {
        private IInventory<ScriptableItem> _inventory;
        [Tooltip("Used only for production and resource gathering sites")]
        [SerializeField] private List<Pair<ScriptableItem, int>> _itemCapacities;
        [Tooltip("Used only for main storages")]
        [SerializeField] private int _maxCapacity;

        private void Awake()
        {
            Building building = GetComponent<Building>();

            if (building.IsHouse)
            {
                _inventory = Inventory<ScriptableItem>.CreateBuilder().Build();
            }
            else if (building.IsConstructionSite)
            {
                Dictionary<ScriptableItem, int> constructionMaterials = building.ScriptableBuilding.BuildingMaterials.ToDictionary();
                _inventory = Inventory<ScriptableItem>.CreateBuilder()
                    .WithPredeterminedItemSet(constructionMaterials)
                    .Build();
            }
            else if (building.IsProductionSite)
            {
                _inventory = Inventory<ScriptableItem>.CreateBuilder()
                    .WithPredeterminedItemSet(_itemCapacities.ToDictionary())
                    .Build();
            }
            else if (building.IsResourceGatheringSite)
            {
                _inventory = Inventory<ScriptableItem>.CreateBuilder()
                    .WithPredeterminedItemSet(_itemCapacities.ToDictionary())
                    .Build();
            }
            else if (building.IsMainStorage)
            {
                _inventory = Inventory<ScriptableItem>.CreateBuilder()
                    .WithLimitedCapacity(_maxCapacity, (ScriptableItem item) => item.ItemSize)
                    .Build();
            }
        }


        private void OnValidate()
        {
            // ToDo:
            // Edit inventory parameters for production sites and resource gathering site to contain exactly
            // the set of items needed and produced
        }

        
        public Dictionary<ScriptableItem, int> AvailableItemsToTake()
        {
            Building building = GetComponent<Building>();

            if (building.IsHouse || building.IsConstructionSite)
            {
                return new Dictionary<ScriptableItem, int>();
            }

            if (building.IsProductionSite)
            {
                Dictionary<ScriptableItem, int> items = _inventory.Items();
                foreach (Recipe recipe in GetComponent<ProductionSite>().AvailableRecipes)
                {
                    foreach (ScriptableItem item in recipe.RequiredItems.Keys)
                    {
                        if (items.ContainsKey(item))
                        {
                            items.Remove(item);
                        }
                    }
                }
                return items;
            }

            if (building.IsResourceGatheringSite)
            {
                return _inventory.Items();
            }

            if (building.IsMainStorage)
            {
                return _inventory.Items();
            }

            Debug.LogError("Unsupported building type");
            return null;
        }

        /// <summary>
        /// Adds as many items as possible to the storage, up to the given quantity or until storage is full.
        /// </summary>Inventory
        /// <param name="item">The item to add to the storage</param>
        /// <param name="quantity">The maximum amount to add</param>
        /// <returns>The quantity actually added</returns>
        public int AddAsManyAsPossible(ScriptableItem item, int quantity)
        {
            int addedQuantity = GetCapacityForItem(item, quantity);
            _inventory.AddItems(item, addedQuantity);
            return addedQuantity;
        }

        public int GetCapacityForItem(ScriptableItem item, int maxAmount)
        {
            int minAmount = 0;
            
            while (minAmount < maxAmount)
            {
                int middle = (int)Mathf.Ceil((float)(maxAmount + minAmount) / 2);

                if (CanAddItems(item, middle))
                {
                    minAmount = middle;
                }
                else
                {
                    maxAmount = middle - 1;
                }
            }

            return minAmount;
        }

        public bool CanAddItems(Dictionary<ScriptableItem, int> items)
        {
            return _inventory.CanAddItems(items);
        }
        public bool CanAddItems(ScriptableItem item, int count) => CanAddItems(new Dictionary<ScriptableItem, int>() { { item, count } });
        public bool CanAddItem(ScriptableItem item) => CanAddItems(item, 1);

        public void AddItems(Dictionary<ScriptableItem, int> items)
        {
            _inventory.AddItems(items);
        }
        public void AddItems(ScriptableItem item, int count) => AddItems(new Dictionary<ScriptableItem, int>() { { item, count } });
        public void AddItem(ScriptableItem item) => AddItems(item, 1);

        public bool ContainsItems(Dictionary<ScriptableItem, int> items)
        {
            return _inventory.ContainsItems(items);
        }

        public void RemoveItems(Dictionary<ScriptableItem, int> items)
        {
            _inventory.RemoveItems(items);
        }
        public void RemoveItems(ScriptableItem item, int count) => RemoveItems(new Dictionary<ScriptableItem, int>() { { item, count } });

        public int GetItemCount(ScriptableItem item)
        {
            return _inventory.GetItemCount(item);
        }

        public bool IsEmpty()
        {
            return _inventory.IsEmpty();
        }

        public Dictionary<ScriptableItem, int> Items()
        {
            return _inventory.Items();
        }
    }
}
