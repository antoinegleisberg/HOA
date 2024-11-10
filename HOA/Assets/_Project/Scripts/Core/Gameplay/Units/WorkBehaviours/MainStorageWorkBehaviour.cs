using antoinegleisberg.Types;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace antoinegleisberg.HOA.Core
{
    public class MainStorageWorkBehaviour : BaseWorkBehaviour
    {
        public MainStorageWorkBehaviour(Citizen citizen) : base(citizen) { }

        public override IEnumerator ExecuteWork()
        {
            yield return _citizen.StartCoroutine(CollectClosestProductionBuildingProduction());
        }

        private IEnumerator CollectClosestProductionBuildingProduction()
        {
            Building buildingToCollectFrom = BuildingsDB.Instance.GetAllBuildings()
                .Where((Building b) => b.IsProductionSite || b.IsResourceGatheringSite)
                .MinBy((Building b) => Vector3.Distance(b.transform.position, _citizen.transform.position));

            if (buildingToCollectFrom == null)
            {
                yield break;
            }

            IReadOnlyDictionary<ScriptableItem, int> items = buildingToCollectFrom.GetComponent<Storage>().AvailableItemsToTake();

            yield return _citizen.StartCoroutine(_citizen.StoreHighestCapacityItemToMainStorage(items.Keys, buildingToCollectFrom.GetComponent<Storage>()));
        }
    }
}
