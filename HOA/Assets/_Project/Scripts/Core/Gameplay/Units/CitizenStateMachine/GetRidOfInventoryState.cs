using antoinegleisberg.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    public class GetRidOfInventoryState : BaseState<Citizen>
    {
        public override void EnterState(Citizen citizen)
        {
            citizen.StartCoroutine(GetRidOfInventoryCoroutine(citizen));
        }

        public override void ExitState(Citizen citizen) { }

        public override void UpdateState(Citizen citizen) { }

        private IEnumerator GetRidOfInventoryCoroutine(Citizen citizen)
        {
            while (citizen.CarriesItems)
            {
                foreach (KeyValuePair<ScriptableItem, int> item in citizen.CarriedItems)
                {
                    yield return citizen.StartCoroutine(TryToGetRidOfItem(citizen,item.Key, item.Value));
                }
                yield return new WaitForSeconds(1);
            }
            citizen.SwitchState(citizen.HomeState);
        }

        private IEnumerator TryToGetRidOfItem(Citizen citizen, ScriptableItem item, int amount)
        {
            ItemStorageInfo itemStorageInfo = BuildingsDB.Instance.GetAvailableMainStorage(item, amount);

            if (itemStorageInfo.Availability.Count == 0)
            {
                yield break;
            }

            Storage target = FindBestPossibleStorage(citizen.transform.position, itemStorageInfo, item, amount);

            if (target == null)
            {
                throw new System.Exception("How can this happen, the method should have exited earlier");
            }

            yield return citizen.StartCoroutine(citizen.MoveToBuilding(target.GetComponent<Building>()));

            int itemsAdded = target.AddAsManyAsPossible(item, amount);
            citizen.DepositItem(item, itemsAdded);
        }

        private Storage FindBestPossibleStorage(Vector3 position, ItemStorageInfo itemStorageInfo, ScriptableItem item, int amount)
        {
            // Step 1: find the storage with enough capacity, and take the closest
            List<KeyValuePair<Storage, int>> storagesWithEnoughCapacity = itemStorageInfo.Availability.Where(kvp => kvp.Value >= amount).ToList();
            if (storagesWithEnoughCapacity.Count > 0)
            {
                Storage closestStorage = storagesWithEnoughCapacity.FirstOrDefault().Key;
                float minDistance = Vector3.Distance(closestStorage.transform.position, position);
                foreach (KeyValuePair<Storage, int> kvp in storagesWithEnoughCapacity)
                {
                    float distance = Vector3.Distance(kvp.Key.transform.position, position);
                    if (distance < minDistance)
                    {
                        closestStorage = kvp.Key;
                        minDistance = distance;
                    }
                }
                return closestStorage;
            }

            // Step 2: if no storage has enough capacity for all items, just pick the closest one
            else
            {
                Storage closestStorage = itemStorageInfo.Availability.FirstOrDefault().Key;
                float minDistance = Vector3.Distance(closestStorage.transform.position, position);
                foreach (KeyValuePair<Storage, int> kvp in itemStorageInfo.Availability)
                {
                    float distance = Vector3.Distance(kvp.Key.transform.position, position);
                    if (distance < minDistance)
                    {
                        closestStorage = kvp.Key;
                        minDistance = distance;
                    }
                }
                return closestStorage;
            }
        }
    }
}
