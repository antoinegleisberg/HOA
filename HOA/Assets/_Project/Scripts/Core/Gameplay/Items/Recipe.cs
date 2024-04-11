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
                return _requiredItems.ToDictionary();
            }
        }
        
        public Dictionary<ScriptableItem, int> ProducedItems {
            get
            {
                return _producedItems.ToDictionary();
            }
        }
    }
}
