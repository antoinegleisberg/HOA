using antoinegleisberg.StateMachine;
using antoinegleisberg.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class WorkingState : BaseState<Citizen>
    {
        private Citizen _citizen;

        public override void EnterState(Citizen citizen)
        {
            _citizen = citizen;
            citizen.StartCoroutine(WorkCoroutine());
        }

        public override void ExitState(Citizen citizen)
        {

        }

        public override void UpdateState(Citizen citizen)
        {

        }

        private Storage WorkplaceStorage { get => _citizen.Workplace.GetComponent<Storage>(); }

        private IEnumerator WorkCoroutine()
        {
            yield return _citizen.MoveToBuilding(_citizen.Workplace.GetComponent<Building>());
            
            ProductionSite productionSite = _citizen.Workplace.GetComponent<ProductionSite>();
            float productionTime = productionSite.GetRecipe().ProductionTime;
            Recipe recipe = productionSite.GetRecipe();

            float startTime = Time.time;

            while (Time.time - startTime < _citizen.TimeAtWork)
            {
                if (!WorkplaceStorage.ContainsItems(recipe.RequiredItems))
                {
                    Debug.Log("Not enough materials, going to get some");
                    yield return _citizen.StartCoroutine(GetMaterialsCoroutine());
                    continue;
                }
                if (!WorkplaceStorage.CanAddItems(recipe.ProducedItems))
                {
                    Debug.Log("No more space for production, need to put some stuff away");
                    yield return _citizen.StartCoroutine(StoreMaterialsCoroutine());
                    continue;
                }

                WorkplaceStorage.RemoveItems(recipe.RequiredItems);

                yield return new WaitForSeconds(productionTime);

                WorkplaceStorage.AddItems(recipe.ProducedItems);

                foreach (KeyValuePair<ScriptableItem, int> producedItem in recipe.ProducedItems)
                {
                    Debug.Log($"Produced {producedItem.Value} {producedItem.Key.Name}");
                }
            }

            _citizen.SwitchState(_citizen.WanderingState);
        }

        private IEnumerator GetMaterialsCoroutine()
        {
            yield return null;
        }

        private IEnumerator StoreMaterialsCoroutine()
        {
            Pair<ScriptableItem, int> itemToStore = GetItemToStoreAway();
            ScriptableItem item = itemToStore.First;
            int quantity = itemToStore.Second;
            if (item == null)
            {
                Debug.LogError("No item to store away !");
                yield break;
            }

            MainStorage mainStorage = BuildingsDB.Instance.GetAvailableMainStorage(item);

            if (mainStorage == null)
            {
                Debug.Log("No main storage available !");
                yield break;
            }

            yield return _citizen.StartCoroutine(_citizen.MoveToBuilding(mainStorage.GetComponent<Building>()));

            int addedQuantity = mainStorage.AddAsManyAsPossible(item, quantity);
            WorkplaceStorage.RemoveItems(item, addedQuantity);

            yield return _citizen.StartCoroutine(_citizen.MoveToBuilding(_citizen.Workplace.GetComponent<Building>()));
        }

        private Pair<ScriptableItem, int> GetItemToStoreAway()
        {
            Dictionary<ScriptableItem, int> availableItemsToStore = _citizen.Workplace.GetComponent<Storage>().AvailableItemsToTake();
            if (availableItemsToStore.Count == 0)
            {
                Debug.LogError("Storage is full, but there is nothing to move away !");
                return new Pair<ScriptableItem, int>(null, 0);
            }
            ScriptableItem item = null;
            int quantity = 0;
            foreach (KeyValuePair<ScriptableItem, int> kvp in availableItemsToStore)
            {
                if (kvp.Value > quantity)
                {
                    item = kvp.Key;
                    quantity = kvp.Value;
                }
            }
            
            return new Pair<ScriptableItem, int>(item, quantity);
        }
    }
}
