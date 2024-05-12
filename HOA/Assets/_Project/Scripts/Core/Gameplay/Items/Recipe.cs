using antoinegleisberg.Types;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "ScriptableObjects/Recipe")]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private List<Pair<ScriptableItem, int>> _requiredItems;
        [SerializeField] private List<Pair<ScriptableItem, int>> _producedItems;

        [field: SerializeField] public string Name { get; private set; }

        [field: SerializeField] public float ProductionTime { get; private set; }

        public Dictionary<ScriptableItem, int> RequiredItems => _requiredItems.ToDictionary();

        public Dictionary<ScriptableItem, int> ProducedItems => _producedItems.ToDictionary();
    }
}
