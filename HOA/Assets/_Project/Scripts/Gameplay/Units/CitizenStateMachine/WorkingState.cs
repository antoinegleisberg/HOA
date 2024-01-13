using antoinegleisberg.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class WorkingState : BaseState<Citizen>
    {
        public override void EnterState(Citizen citizen)
        {
            citizen.StartCoroutine(WorkCoroutine(citizen));
        }

        public override void ExitState(Citizen citizen)
        {

        }

        public override void UpdateState(Citizen citizen)
        {

        }

        private IEnumerator WorkCoroutine(Citizen citizen)
        {
            yield return citizen.MoveToBuilding(citizen.Workplace.GetComponent<Building>());

            Storage workplaceStorage = citizen.Workplace.GetComponent<Storage>();
            ProductionSite productionSite = citizen.Workplace.GetComponent<ProductionSite>();
            float productionTime = productionSite.GetRecipe().ProductionTime;
            Recipe recipe = productionSite.GetRecipe();

            float startTime = Time.time;

            while (Time.time - startTime < citizen.TimeAtWork)
            {
                if (!workplaceStorage.Inventory.ContainsItems(recipe.RequiredItems))
                {
                    Debug.Log("Not enough materials, going to get some");
                    yield return citizen.StartCoroutine(GetMaterialsCoroutine(citizen));
                    continue;
                }
                if (!workplaceStorage.Inventory.CanAddItems(recipe.ProducedItems))
                {
                    Debug.Log("No more space for production, need to put some stuff away");
                    yield return citizen.StartCoroutine(StoreMaterialsCoroutine(citizen));
                    continue;
                }

                workplaceStorage.Inventory.RemoveItems(recipe.RequiredItems);

                yield return new WaitForSeconds(productionTime);

                foreach (KeyValuePair<ScriptableItem, int> producedItem in recipe.ProducedItems)
                {
                    workplaceStorage.Inventory.AddItems(producedItem.Key, producedItem.Value);
                    Debug.Log($"Produced {producedItem.Value} {producedItem.Key.Name}");
                }
            }

            citizen.SwitchState(citizen.WanderingState);
        }

        private IEnumerator GetMaterialsCoroutine(Citizen citizen)
        {
            yield return null;
        }

        private IEnumerator StoreMaterialsCoroutine(Citizen citizen)
        {
            yield return null;
        }
    }
}
