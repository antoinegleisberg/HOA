using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class ProductionSiteWorkBehaviour : BaseWorkBehaviour
    {
        private Storage _workplaceStorage { get => _citizen.Workplace.GetComponent<Storage>(); }
        
        public ProductionSiteWorkBehaviour(Citizen citizen) : base(citizen) { }

        public override IEnumerator ExecuteWork()
        {
            ProductionSite productionSite = _citizen.Workplace.GetComponent<ProductionSite>();
            Recipe recipe = productionSite.GetRecipe();

            if (!_workplaceStorage.ContainsItems(recipe.RequiredItems))
            {
                yield return _citizen.StartCoroutine(GetMaterialsCoroutine(recipe));
                yield break;
            }
            if (!_workplaceStorage.CanAddItems(recipe.ProducedItems))
            {
                yield return _citizen.StartCoroutine(StoreItemsToMainStorage(recipe.ProducedItems.Keys));
                yield break;
            }

            yield return _citizen.StartCoroutine(ProduceCoroutine(recipe));
        }

        private IEnumerator GetMaterialsCoroutine(Recipe recipe)
        {
            yield return _citizen.StartCoroutine(_citizen.GetItemsFromAvailableStorage(recipe.RequiredItems, _workplaceStorage.Items(), _workplaceStorage));
        }

        private IEnumerator StoreItemsToMainStorage(IEnumerable<ScriptableItem> items)
        {
            yield return _citizen.StartCoroutine(_citizen.StoreLimitingItemsToMainStorage(items, _workplaceStorage));

            yield return _citizen.StartCoroutine(_citizen.MoveToBuilding(_citizen.Workplace.GetComponent<Building>()));
        }

        private IEnumerator ProduceCoroutine(Recipe recipe)
        {
            _workplaceStorage.RemoveItems(recipe.RequiredItems);
            _workplaceStorage.AddItems(recipe.ProducedItems);
            yield return new WaitForSeconds(recipe.ProductionTime);

            foreach (KeyValuePair<ScriptableItem, int> producedItem in recipe.ProducedItems)
            {
                Debug.Log($"Produced {producedItem.Value} {producedItem.Key.Name}");
            }
        }
    }
}
