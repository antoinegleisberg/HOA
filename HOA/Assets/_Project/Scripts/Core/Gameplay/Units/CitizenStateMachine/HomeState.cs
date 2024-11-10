using antoinegleisberg.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace antoinegleisberg.HOA.Core
{
    public class HomeState : BaseState<Citizen>
    {
        public override void EnterState(Citizen citizen)
        {
            TryToClaimHouse(citizen);

            if (citizen.Home != null)
            {
                citizen.StartCoroutine(HomeCoroutine(citizen));
            }
            else
            {
                citizen.SwitchState(citizen.WanderingState);
            }
        }

        public override void ExitState(Citizen citizen)
        {

        }

        public override void UpdateState(Citizen citizen)
        {

        }

        private static void TryToClaimHouse(Citizen citizen)
        {
            if (citizen.Home == null)
            {
                House house = BuildingsDB.Instance.GetAvailableHouse();
                if (house != null)
                {
                    citizen.ClaimHouse(house);
                }
            }
        }

        private IEnumerator HomeCoroutine(Citizen citizen)
        {
            yield return citizen.StartCoroutine(citizen.MoveToBuilding(citizen.Home.GetComponent<Building>()));

            TryToSpawnBaby(citizen);

            float startTime = Time.time;
            float timeAtHome = citizen.StaticData.TimeAtHome;
            while (Time.time - startTime < timeAtHome)
            {
                yield return citizen.StartCoroutine(ReplenishNeeds(citizen));
                yield return new WaitForSeconds(1);

                if (citizen.CarriesItems)
                {
                    citizen.SwitchState(citizen.GetRidOfInventoryState);
                    yield break;
                }
            }
            
            if (citizen.Workplace == null)
            {
                citizen.SwitchState(citizen.SearchWorksiteState);
            }
            else
            {
                citizen.SwitchState(citizen.WorkingState);
            }
        }

        private void TryToSpawnBaby(Citizen citizen)
        {
            // At some point, replace this with GlobalPopulation.IsFull ?
            if (citizen.Home.IsFull)
            {
                return;
            }
            
            if (!citizen.CanSpawnBaby)
            {
                return;
            }

            float probabilityToSpawnBaby = citizen.StaticData.ProbabilityToSpawnBaby;
            if (Random.value > probabilityToSpawnBaby)
            {
                return;
            }

            foreach (Citizen otherCitizen in citizen.Home.Residents)
            {
                if (otherCitizen == citizen)
                {
                    continue;
                }
                
                if (otherCitizen.CanSpawnBaby)
                {
                    Citizen newCitizen = UnitManager.Instance.SpawnCitizen(citizen.Home.transform.position);
                    citizen.StartCooldownForNextBaby();
                    otherCitizen.StartCooldownForNextBaby();
                    newCitizen.ClaimHouse(citizen.Home);
                    newCitizen.SetCurrentBuilding(citizen.Home.GetComponent<Building>());
                    return;
                }
            }
        }

        // Replace this with GetItemThatFulfillsNeed(Citizen citizen, Need need)
        // and also create the Need class accordingly (scriptable object ?) => yes?
        // This allows generic behaviour
        private IEnumerator ReplenishNeeds(Citizen citizen)
        {
            // Do we need to order needs here, instead of just satisfying the first one ?
            foreach (ScriptableCitizenNeed need in citizen.Needs.Keys)
            {
                if (citizen.Needs[need] < 20)
                {
                    ScriptableItem item = citizen.Home.GetComponent<Storage>().GetNeedReplenishingItem(need);

                    if (item == null)
                    {
                        yield return citizen.StartCoroutine(GetNeedReplenishingItem(citizen, need));
                        yield break;
                    }
                    citizen.Home.GetComponent<Storage>().RemoveItems(item, 1);
                    citizen.ReplenishNeed(item);

                    // Only replenish one item every time the coroutine is called
                    yield break;
                }
            }
        }

        private IEnumerator GetNeedReplenishingItem(Citizen citizen, ScriptableCitizenNeed need)
        {
            // What about the possibility to get water from storage ??

            // ToDo - adapt to now generic needs
            yield break;

            ResourceSite waterCollectionSite = Object.FindObjectsOfType<ResourceSite>()
                .Where(rs => rs.ScriptableResourceSite.ResourceSiteType == ResourceSiteType.Water)
                .FirstOrDefault();

            if (waterCollectionSite == null)
            {
                yield break;
            }

            if (waterCollectionSite.GetComponent<Building>() != null)
            {
                yield return citizen.StartCoroutine(citizen.MoveToBuilding(waterCollectionSite.GetComponent<Building>()));
            }
            else
            {
                yield return citizen.StartCoroutine(citizen.MoveToPosition(waterCollectionSite.transform.position));
            }

            citizen.PickUpItems(waterCollectionSite.Harvest());

            yield return citizen.StartCoroutine(citizen.MoveToBuilding(citizen.Home.GetComponent<Building>()));

            IReadOnlyDictionary<ScriptableItem, int> depositedItems =  citizen.Home.GetComponent<Storage>().AddAsManyAsPossible(citizen.CarriedItems);
            citizen.DepositItems(depositedItems);
        }
    }
}
