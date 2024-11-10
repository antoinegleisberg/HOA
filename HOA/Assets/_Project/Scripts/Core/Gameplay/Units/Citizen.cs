using antoinegleisberg.Inventory;
using antoinegleisberg.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [RequireComponent(typeof(CitizenMovement), typeof(CitizenItemTransport))]
    public class Citizen : MonoBehaviour
    {
        [field: SerializeField] public ScriptableStaticCitizenData StaticData { get; private set; }
        
        private int _daysSinceLastBaby = 0;

        private IInventory<ScriptableItem> _inventory;
        public IReadOnlyDictionary<ScriptableItem, int> CarriedItems => _inventory.Items();

        [field: SerializeField] public House Home { get; private set; }
        [field: SerializeField] public Workplace Workplace { get; private set; }


        private StateMachine<Citizen> _stateMachine;
        public HomeState HomeState { get; private set; }
        public SearchWorksiteState SearchWorksiteState { get; private set; }
        public WorkingState WorkingState { get; private set; }
        public WanderingState WanderingState { get; private set; }
        public GetRidOfInventoryState GetRidOfInventoryState { get; private set; }

        private CitizenMovement _citizenMovement => GetComponent<CitizenMovement>();
        private CitizenItemTransport _citizenItemTransport => GetComponent<CitizenItemTransport>();
        
        public Dictionary<ScriptableCitizenNeed, int> Needs;

        private IEnumerable<BaseState<Citizen>> _states;

        public bool IsInBuilding => _citizenMovement.IsInBuilding;
        public Building CurrentBuilding => _citizenMovement.CurrentBuilding;
        public bool CanSpawnBaby => _daysSinceLastBaby >= StaticData.CooldownInDaysBeforeNextBaby;
        public bool CarriesItems => !_inventory.IsEmpty();

        private void Awake()
        {
            HomeState = new HomeState();
            SearchWorksiteState = new SearchWorksiteState();
            WanderingState = new WanderingState();
            WorkingState = new WorkingState();
            GetRidOfInventoryState = new GetRidOfInventoryState();

            _stateMachine = new StateMachine<Citizen>(this, HomeState);

            _states = new BaseState<Citizen>[]
            {
                HomeState,
                SearchWorksiteState,
                WanderingState,
                WorkingState,
                GetRidOfInventoryState
            };

            _inventory = Inventory<ScriptableItem>.CreateBuilder()
                .WithLimitedCapacity(StaticData.InventorySize, (ScriptableItem si) => si.ItemSize)
                .Build();

            Needs = new Dictionary<ScriptableCitizenNeed, int>(
                ScriptableCitizenNeedDB.GetAllNeeds()
                .Select((ScriptableCitizenNeed need) => new KeyValuePair<ScriptableCitizenNeed, int>(need, 100)));
        }

        private void Start()
        {
            TimeManager.Instance.OnDayChanged += DepleteAllNeeds;
            TimeManager.Instance.OnDayChanged += () => _daysSinceLastBaby = Mathf.Min(_daysSinceLastBaby + 1, StaticData.CooldownInDaysBeforeNextBaby);
        }

        private void OnDestroy()
        {
            TimeManager.Instance.OnDayChanged -= DepleteAllNeeds;
            TimeManager.Instance.OnDayChanged -= () => _daysSinceLastBaby = Mathf.Min(_daysSinceLastBaby + 1, StaticData.CooldownInDaysBeforeNextBaby);
        }

        private void DepleteAllNeeds()
        {
            foreach (ScriptableCitizenNeed need in Needs.Keys)
            {
                Needs[need] = Mathf.Max(Needs[need] - 5, 0);
            }
        }

        public void StartCooldownForNextBaby()
        {
            _daysSinceLastBaby = 0;
        }

        public bool CanPickUpItems(IReadOnlyDictionary<ScriptableItem, int> items)
        {
            return _inventory.CanAddItems(items);
        }

        public void PickUpItem(ScriptableItem item, int amount)
        {
            _inventory.AddItems(item, amount);
        }

        public void PickUpItems(IReadOnlyDictionary<ScriptableItem, int> items)
        {
            _inventory.AddItems(items);
        }

        public void DepositItem(ScriptableItem item, int amount)
        {
            DepositItems(new Dictionary<ScriptableItem, int> { { item, amount } });
        }

        public void DepositItems(IReadOnlyDictionary<ScriptableItem, int> items)
        {
            _inventory.RemoveItems(items);
        }

        public void DropAllItems()
        {
            _inventory.RemoveItems(_inventory.Items());
        }

        public void SetCurrentBuilding(Building building)
        {
            if (building == null)
            {
                return;
            }
            _citizenMovement.IsInBuilding = true;
            _citizenMovement.CurrentBuilding = building;
        }

        public void ReplenishNeed(ScriptableItem replenisher)
        {
            foreach (KeyValuePair<ScriptableCitizenNeed, int> needReplenish in replenisher.NeedsReplenish)
            {
                Needs[needReplenish.Key] = Mathf.Min(Needs[needReplenish.Key] + needReplenish.Value, 100);
            }
        }

        public void ClaimHouse(House house)
        {
            if (Home != null)
            {
                Home.RemoveResident(this);
            }
            Home = house;
            Debug.Log("Claiming house");
            house.AddResident(this);
        }

        public void ClaimWorkplace(Workplace workplace)
        {
            if (Workplace != null)
            {
                SwitchState(HomeState);
                Workplace.RemoveWorker(this);
            }
            Workplace = workplace;
            Debug.Log("Claiming workplace");
            workplace?.AddWorker(this);
        }

        public void SwitchState(BaseState<Citizen> newState)
        {
            // I'm not 100% sure about stopping all coroutines here, I believe this could break some stuff
            // However, I think it may be necessary in order to allow the citizen to reset behaviour when we
            // forcefully set his state when loading a save file from memory
            StopAllCoroutines();
            _citizenMovement.StopAllCoroutines();
            _citizenItemTransport.StopAllCoroutines();
            Debug.Log("Switching to state " + newState.GetType().ToString());
            if (newState == GetRidOfInventoryState)
            {
                _stateMachine.PushState(newState);
            }
            else if (_stateMachine.GetCurrentState() == GetRidOfInventoryState)
            {
                Debug.Log("Ignoring new state and switching back to previous state (before GetRidOfInventory)");
                _stateMachine.PopState();
            }
            else
            {
                _stateMachine.SwitchState(newState);
            }
        }

        public string GetCurrentStateAsString()
        {
            return _stateMachine.GetCurrentState().GetType().ToString();
        }

        public void SetStateFromString(string stateName)
        {
            foreach (BaseState<Citizen> possibleState in _states)
            {
                if (stateName == possibleState.GetType().ToString())
                {
                    SwitchState(possibleState);
                    return;
                }
            }
            throw new ArgumentException($"The state {stateName} could not be identified !");
        }

        public IEnumerator MoveToPosition(Vector3 targetPosition)
        {
            yield return StartCoroutine(_citizenMovement.MoveToPosition(targetPosition));
        }

        public IEnumerator MoveToBuilding(Building target)
        {
            yield return StartCoroutine(_citizenMovement.MoveToBuilding(target));
        }
        
        public IEnumerator StoreHighestCapacityItemToMainStorage(IEnumerable<ScriptableItem> items, Storage storageToTakeFrom)
        {
            yield return StartCoroutine(_citizenItemTransport.StoreHighestCapacityItemToMainStorage(items, storageToTakeFrom));
        }

        public IEnumerator GetItemsFromAvailableStorage(IReadOnlyDictionary<ScriptableItem, int> requiredItems, IReadOnlyDictionary<ScriptableItem, int> availableItems, Storage storageToTakeTo)
        {
            yield return StartCoroutine(_citizenItemTransport.GetItemsFromAvailableStorage(requiredItems, availableItems, storageToTakeTo));
        }
    }
}
