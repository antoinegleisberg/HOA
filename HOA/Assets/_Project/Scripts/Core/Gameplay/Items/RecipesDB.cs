using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    public static class RecipesDB
    {
        private static readonly string _path = "Recipes";

        private static Dictionary<string, Recipe> __recipes;

        private static Dictionary<string, Recipe> _items {
            get
            {
                if (__recipes == null)
                {
                    Init();
                }
                return __recipes;
            }
        }

        public static Recipe GetRecipeByName(string name)
        {
            if (!_items.ContainsKey(name))
            {
                Debug.LogError("ItemsDB does not contain an item with the name " + name);
                return null;
            }

            return _items[name];
        }

        public static void Reload()
        {
            Init();
        }

        private static void Init()
        {
            __recipes = new Dictionary<string, Recipe>();

            Recipe[] recipes = Resources.LoadAll<Recipe>(_path);

            foreach (Recipe recipe in recipes)
            {
                if (_items.ContainsKey(recipe.Name))
                {
                    Debug.LogError("RecipesDB already contains an item with the name " + recipe.Name);
                    continue;
                }
                _items.Add(recipe.Name, recipe);
            }
        }
    }
}
