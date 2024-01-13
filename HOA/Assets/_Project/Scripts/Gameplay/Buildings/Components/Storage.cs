using UnityEngine;
using antoinegleisberg.Inventory;
using System.Collections.Generic;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building))]
    public class Storage : MonoBehaviour
    {
        public IInventory<ScriptableItem> Inventory { get; private set; }

        [SerializeField] private int _maxInventoryCapacity;

        private void Awake()
        {
            Inventory = InventoryBuilder.CreateLimitedCapacityInventory(_maxInventoryCapacity, (ScriptableItem item) => item.ItemSize);
        }

        public Dictionary<ScriptableItem, int> AvailableItemsToTake()
        {
            if (GetComponent<House>() != null || GetComponent<BuildSite>() != null)
            {
                return new Dictionary<ScriptableItem, int>();
            }
            
            if (GetComponent<ProductionSite>() != null)
            {
                Dictionary<ScriptableItem, int> items = Inventory.Items();
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
            
            if (GetComponent<ResourceGatheringSite>() != null)
            {
                return Inventory.Items();
            }

            // It's a main storage
            return Inventory.Items();
        }
    }
}
