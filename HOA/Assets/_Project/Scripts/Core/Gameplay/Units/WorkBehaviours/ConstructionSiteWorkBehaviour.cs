using System.Collections;
using System.Collections.Generic;
using System.Linq;
using antoinegleisberg.Types;

namespace antoinegleisberg.HOA.Core
{
    public class ConstructionSiteWorkBehaviour : BaseWorkBehaviour
    {
        private Storage _workplaceStorage => _citizen.Workplace.GetComponent<Storage>();

        public ConstructionSiteWorkBehaviour(Citizen citizen) : base(citizen) { }

        public override IEnumerator ExecuteWork()
        {
            ConstructionSite constructionSite = _citizen.Workplace.GetComponent<ConstructionSite>();

            if (constructionSite.CanBuild())
            {
                // Unassign the citizen from the construction site which is about to be constructed
                // He will not work there anymore once it is built
                _citizen.ClaimWorkplace(null);
                Building newBuilding = constructionSite.FinishConstruction();
                if (newBuilding.IsWorkplace)
                {
                    // Change to do this only if we define that a citizen should take a job at the building he
                    // just built => add a setting for that
                    _citizen.ClaimWorkplace(newBuilding.GetComponent<Workplace>());
                }
                yield break;
            }

            Dictionary<ScriptableItem, int> requiredItems = constructionSite.GetComponent<Building>().ScriptableBuilding.BuildingMaterials.ToDictionary();
            yield return _citizen.StartCoroutine(_citizen.GetItemsFromAvailableStorage(requiredItems, _workplaceStorage.Items(), _workplaceStorage));
        }
    }
}
