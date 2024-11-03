using antoinegleisberg.Inventory;
using antoinegleisberg.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [RequireComponent(typeof(Building))]
    public class Storage : MonoBehaviour, IInventory<ScriptableItem>
    {
        public bool InitializedStorage { get; private set; }
        
        private IInventory<ScriptableItem> _inventory;

        [SerializeField] private ScriptableStorage _scriptableStorage;

        private IReadOnlyDictionary<ScriptableItem, int> _itemCapacities => _scriptableStorage.ItemCapacities;
        private int _maxCapacity => _scriptableStorage.MaxCapacity;

        private void Awake()
        {
            InitializedStorage = false;
            // Temporary, fake inventory with 0 capacity while the actual one is not initialized yet
            _inventory = Inventory<ScriptableItem>.CreateBuilder().WithLimitedCapacity(0, (ScriptableItem item) => 1).Build();
            StartCoroutine(InitialiseStorage());
        }

        private void OnValidate()
        {
            Building building = GetComponent<Building>();

            if (building.IsHouse || building.IsConstructionSite)
            {
                if (_scriptableStorage != null)
                {
                    Debug.LogWarning("A House or Construction Site should not have a ScriptableStorage attached to it. The ScriptableStorage will be ignored");
                }
            }
            else if (building.IsMainStorage)
            {
                if (_scriptableStorage == null)
                {
                    throw new Exception("A Main Storage must have a ScriptableStorage attached to it.");
                }
                if (_maxCapacity <= 0)
                {
                    Debug.LogWarning("Main Storage capacity is set to 0. Is this intended?");
                }
                if (_itemCapacities.Count > 0)
                {
                    throw new Exception("Main Storage has item capacities set. These will be ignored.");
                }
            }
            else if (building.IsProductionSite || building.IsResourceGatheringSite)
            {
                if (_scriptableStorage == null)
                {
                    throw new Exception("A Production Site or Resource Gathering Site must have a ScriptableStorage attached to it.");
                }
                if (_maxCapacity != 0)
                {
                    throw new Exception("Production Site or Resource Gathering Site max storage capacity is set to a value.");
                }
            }
            else
            {
                throw new Exception("Building type not recognised");
            }
        }

        public ScriptableItem GetDrink()
        {
            foreach (ScriptableItem item in _inventory.Items().Keys)
            {
                if (item.ThirstReplenish > 0)
                {
                    return item;
                }
            }
            return null;
        }
        
        public ScriptableItem GetFood()
        {
            foreach (ScriptableItem item in _inventory.Items().Keys)
            {
                if (item.HungerReplenish > 0)
                {
                    return item;
                }
            }
            return null;
        }

        public IReadOnlyDictionary<ScriptableItem, int> AvailableItemsToTake()
        {
            Building building = GetComponent<Building>();

            if (building.IsHouse || building.IsConstructionSite)
            {
                return new Dictionary<ScriptableItem, int>();
            }

            if (building.IsProductionSite)
            {
                Dictionary<ScriptableItem, int> items = new Dictionary<ScriptableItem, int>(_inventory.Items());
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

        /// <summary>
        /// Adds as many items as possible to the storage, up to the given quantity or until storage is full.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public IReadOnlyDictionary<ScriptableItem, int> AddAsManyAsPossible(IReadOnlyDictionary<ScriptableItem, int> items)
        {
            Dictionary<ScriptableItem, int> addedItems = new Dictionary<ScriptableItem, int>();

            foreach (KeyValuePair<ScriptableItem, int> item in items)
            {
                addedItems.Add(item.Key, AddAsManyAsPossible(item.Key, item.Value));
            }

            return addedItems;
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

        public bool CanAddItems(IReadOnlyDictionary<ScriptableItem, int> items) => _inventory.CanAddItems(items);
        public bool CanAddItems(ScriptableItem item, int count) => CanAddItems(new Dictionary<ScriptableItem, int>() { { item, count } });
        public bool CanAddItem(ScriptableItem item) => CanAddItems(item, 1);

        public void AddItems(IReadOnlyDictionary<ScriptableItem, int> items) => _inventory.AddItems(items);
        public void AddItems(ScriptableItem item, int count) => AddItems(new Dictionary<ScriptableItem, int>() { { item, count } });
        public void AddItem(ScriptableItem item) => AddItems(item, 1);

        public bool ContainsItems(IReadOnlyDictionary<ScriptableItem, int> items) => _inventory.ContainsItems(items);

        public void RemoveItems(IReadOnlyDictionary<ScriptableItem, int> items) => _inventory.RemoveItems(items);
        public void RemoveItems(ScriptableItem item, int count) => RemoveItems(new Dictionary<ScriptableItem, int>() { { item, count } });

        public int GetItemCount(ScriptableItem item) => _inventory.GetItemCount(item);

        public bool IsEmpty() => _inventory.IsEmpty();

        public IReadOnlyDictionary<ScriptableItem, int> Items() => _inventory.Items();

        private IEnumerator InitialiseStorage()
        {
            Building building = GetComponent<Building>();
            
            yield return new WaitUntil(() => building.ScriptableBuilding != null);
            
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
                    .WithPredeterminedItemSet(_itemCapacities)
                    .Build();
            }
            else if (building.IsResourceGatheringSite)
            {
                _inventory = Inventory<ScriptableItem>.CreateBuilder()
                    .WithPredeterminedItemSet(_itemCapacities)
                    .Build();
            }
            else if (building.IsMainStorage)
            {
                _inventory = Inventory<ScriptableItem>.CreateBuilder()
                    .WithLimitedCapacity(_maxCapacity, (ScriptableItem item) => item.ItemSize)
                    .Build();
            }
            else
            {
                Debug.LogError("Building type not recognized");
            }

            InitializedStorage = true;
        }
    }
}
