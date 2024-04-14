using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building), typeof(Storage))]
    public class ProductionSite : MonoBehaviour
    {
        [SerializeField] private ScriptableProductionSite _scriptableProductionSite;

        public IReadOnlyList<Recipe> AvailableRecipes
        {
            get
            {
                return _scriptableProductionSite.AvailableRecipes;
            }
        }

        public Recipe GetRecipe()
        {
            return AvailableRecipes[0];
        }
    }
}
