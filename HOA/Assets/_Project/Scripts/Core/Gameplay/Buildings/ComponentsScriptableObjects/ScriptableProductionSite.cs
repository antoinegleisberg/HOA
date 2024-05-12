using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName = "NewProductionSite", menuName = "ScriptableObjects/Buildings/Production Site")]
    public class ScriptableProductionSite : ScriptableObject
    {
        [SerializeField] private List<Recipe> _availableRecipes;

        public IReadOnlyList<Recipe> AvailableRecipes => _availableRecipes;
    }
}
