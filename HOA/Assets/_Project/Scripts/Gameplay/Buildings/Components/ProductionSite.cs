using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building))]
    public class ProductionSite : MonoBehaviour
    {
        [field:SerializeField] public List<Recipe> AvailableRecipes { get; private set; }

        public Recipe GetRecipe()
        {
            return AvailableRecipes[0];
        }
    }
}
