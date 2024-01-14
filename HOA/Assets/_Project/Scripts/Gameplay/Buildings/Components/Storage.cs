using antoinegleisberg.Types;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building))]
    public class Storage : MonoBehaviour
    {
        [field: SerializeField] private List<Pair<ScriptableItem, int>> _storageSpaceInitializer;
        private Dictionary<ScriptableItem, int> _storageSpace;
        private Dictionary<ScriptableItem, int> _storedItems;

        private void Awake()
        {
            _storageSpace = new Dictionary<ScriptableItem, int>();
            _storedItems = new Dictionary<ScriptableItem, int>();
            foreach (Pair<ScriptableItem, int> pair in _storageSpaceInitializer)
            {
                _storageSpace.Add(pair.First, pair.Second);
                _storedItems.Add(pair.First, 0);
            }
        }

        public bool ContainsItems(Dictionary<ScriptableItem, int> items)
        {
            foreach (KeyValuePair<ScriptableItem, int> kvp in items)
            {
                if (!_storedItems.ContainsKey(kvp.Key))
                {
                    return false;
                }

                if (_storedItems[kvp.Key] < kvp.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanAddItems(Dictionary<ScriptableItem, int> items)
        {
            foreach (KeyValuePair<ScriptableItem, int> kvp in items)
            {
                if (!_storedItems.ContainsKey(kvp.Key))
                {
                    return false;
                }
                if (_storedItems[kvp.Key] + kvp.Value > _storageSpace[kvp.Key])
                {
                    return false;
                }
            }
            return true;
        }

        public void AddItems(Dictionary<ScriptableItem, int> items)
        {
            foreach (KeyValuePair<ScriptableItem, int> kvp in items)
            {
                if (!_storedItems.ContainsKey(kvp.Key))
                {
                    throw new System.Exception("This item cannot be stored here");
                }
                if (_storedItems[kvp.Key] + kvp.Value > _storageSpace[kvp.Key])
                {
                    throw new System.Exception("Storage is full !");
                }
                _storedItems[kvp.Key] += kvp.Value;
            }
        }

        public void RemoveItems(Dictionary<ScriptableItem, int> items)
        {
            foreach (KeyValuePair<ScriptableItem, int> kvp in items)
            {
                if (!_storedItems.ContainsKey(kvp.Key))
                {
                    throw new System.Exception("Item was not available to remove");
                }
                if (_storedItems[kvp.Key] < kvp.Value)
                {
                    throw new System.Exception("Not enough items to remove");
                }
                _storedItems[kvp.Key] -= kvp.Value;
            }
        }

        public void RemoveItems(ScriptableItem item, int quantity) => RemoveItems(new Dictionary<ScriptableItem, int>() { { item, quantity } });

        public Dictionary<ScriptableItem, int> AvailableItemsToTake()
        {
            if (GetComponent<House>() != null || GetComponent<BuildSite>() != null)
            {
                return new Dictionary<ScriptableItem, int>();
            }

            if (GetComponent<ProductionSite>() != null)
            {
                Dictionary<ScriptableItem, int> items = new Dictionary<ScriptableItem, int>(_storedItems);
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
                return new Dictionary<ScriptableItem, int>(_storedItems);
            }

            return null;
        }
    }
}
