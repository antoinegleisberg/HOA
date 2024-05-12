using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [RequireComponent(typeof(Building), typeof(Storage), typeof(Workplace))]
    public class ProductionSite : MonoBehaviour
    {
        [SerializeField] private ScriptableProductionSite _scriptableProductionSite;

        private Dictionary<Citizen, Recipe> _workersRecipes = new Dictionary<Citizen, Recipe>();

        public IReadOnlyList<Recipe> AvailableRecipes => _scriptableProductionSite.AvailableRecipes;

        public Recipe GetRecipe(Citizen worker)
        {
            if (!_workersRecipes.ContainsKey(worker))
            {
                throw new Exception("This worker is not assigned to this workplace !");
            }
            return _workersRecipes[worker];
        }

        public void SetRecipe(Citizen worker, Recipe recipe)
        {
            if (!AvailableRecipes.Contains(recipe))
            {
                throw new Exception("This recipe is not available at this workplace !");
            }
            _workersRecipes[worker] = recipe;
        }
    }
}
