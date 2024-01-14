using UnityEngine;
using antoinegleisberg.Inventory;
using System.Collections.Generic;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building))]
    public class MainStorage : MonoBehaviour
    {
        public IInventory<ScriptableItem> Inventory { get; private set; }

        [SerializeField] private int _maxInventoryCapacity;

        private void Awake()
        {
            Inventory = InventoryBuilder.CreateLimitedCapacityInventory(_maxInventoryCapacity, (ScriptableItem item) => item.ItemSize);
        }

        /// <summary>
        /// Adds as many items as possible to the storage, up to the given quantity or until storage is full.
        /// </summary>
        /// <param name="item">The item to add to the storage</param>
        /// <param name="quantity">The maximum amount to add</param>
        /// <returns>The quantity actually added</returns>
        public int AddAsManyAsPossible(ScriptableItem item, int quantity)
        {
            int addedQuantity = 0;
            while (addedQuantity < quantity && Inventory.CanAddItems(item, addedQuantity))
            {
                addedQuantity++;
            }
            Inventory.AddItems(item, addedQuantity);
            return addedQuantity;
        }
    }
}
