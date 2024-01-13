using antoinegleisberg.Inventory;
using antoinegleisberg.Types;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "ScriptableObjects/Recipe")]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private List<Pair<ScriptableItem, int>> _requiredItems;
        [SerializeField] private List<Pair<ScriptableItem, int>> _producedItems;

        public float ProductionTime;
        
        public Dictionary<ScriptableItem, int> RequiredItems {
            get
            {
                Dictionary<ScriptableItem, int> requiredItems = new Dictionary<ScriptableItem, int>();
                foreach (Pair<ScriptableItem, int> requiredItem in _requiredItems)
                {
                    requiredItems.Add(requiredItem.First, requiredItem.Second);
                }
                return requiredItems;
            }
        }
        
        public Dictionary<ScriptableItem, int> ProducedItems {
            get
            {
                Dictionary<ScriptableItem, int> producedItems = new Dictionary<ScriptableItem, int>();
                foreach (Pair<ScriptableItem, int> producedItem in _producedItems)
                {
                    producedItems.Add(producedItem.First, producedItem.Second);
                }
                return producedItems;
            }
        }
    }
}
