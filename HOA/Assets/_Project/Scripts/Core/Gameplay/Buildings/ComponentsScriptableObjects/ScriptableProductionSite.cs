using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [CreateAssetMenu(fileName = "NewProductionSite", menuName = "ScriptableObjects/Buildings/Production Site")]
    public class ScriptableProductionSite : ScriptableObject
    {
        public List<Recipe> AvailableRecipes;
    }
}
