using antoinegleisberg.StateMachine;
using System.Collections;
using System.Linq;
using UnityEngine;


namespace antoinegleisberg.HOA.Core
{
    public class HomeState : BaseState<Citizen>
    {
        public override void EnterState(Citizen citizen)
        {
            if (citizen.Home == null)
            {
                House house = BuildingsDB.Instance.GetAvailableHouse();
                if (house != null)
                {
                    citizen.ClaimHouse(house);
                }
            }
            
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

        private IEnumerator HomeCoroutine(Citizen citizen)
        {
            yield return citizen.MoveToBuilding(citizen.Home.GetComponent<Building>());

            TryToSpawnBaby(citizen);

            float startTime = Time.time;
            while (Time.time - startTime < citizen.TimeAtHome)
            {
                yield return citizen.StartCoroutine(GetBeverageAndDrink(citizen));
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
            if (citizen.Home.ResidentsCount >= 2)
            {
                UnitManager.Instance.SpawnCitizen(citizen.Home.transform.position);
            }
        }

        private IEnumerator GetBeverageAndDrink(Citizen citizen)
        {
            ScriptableItem item = citizen.Home.GetComponent<Storage>().GetDrink();
            if (item == null)
            {
                yield return citizen.StartCoroutine(GetWater(citizen));
                yield break;
            }
            citizen.Home.GetComponent<Storage>().RemoveItems(item, 1);
            citizen.ReplenishThirst(item);
        }

        private IEnumerator GetWater(Citizen citizen)
        {
            ResourceSite well = Object.FindObjectsOfType<ResourceSite>()
                .Where(rs => rs.ScriptableResourceSite.ResourceSiteType == ResourceSiteType.Water)
                .FirstOrDefault();

            if (well == null)
            {
                yield return new WaitForSeconds(1.0f);
                yield break;
            }
                

            yield return citizen.StartCoroutine(citizen.MoveToBuilding(well.GetComponent<Building>()));

            yield return citizen.StartCoroutine(citizen.MoveToBuilding(citizen.Home.GetComponent<Building>()));

            citizen.Home.GetComponent<Storage>().AddItems(ScriptableItemsDB.GetItemByName("Water"), 10);
        }
    }
}
