using antoinegleisberg.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [RequireComponent(typeof(CitizenMovement), typeof(CitizenItemTransport), typeof(CitizenNeeds))]
    public class Citizen : MonoBehaviour
    {
        [field: SerializeField] public float WanderingDistance { get; private set; }

        [field: SerializeField] public float TimeAtWork { get; private set; }
        [field: SerializeField] public float TimeAtHome { get; private set; }
        [field: SerializeField] public float TimeWandering { get; private set; }

        [field: SerializeField] public House Home { get; private set; }
        [field: SerializeField] public Workplace Workplace { get; private set; }

        [field: SerializeField] public float ProbabilityToSpawnBaby { get; private set; } = 0.1f;

        private StateMachine<Citizen> _stateMachine;
        public HomeState HomeState { get; private set; }
        public SearchWorksiteState SearchWorksiteState { get; private set; }
        public WorkingState WorkingState { get; private set; }
        public StoringState StoringState { get; private set; }
        public TakingFromStorageState TakingFromStorageState { get; private set; }
        public WanderingState WanderingState { get; private set; }

        private CitizenMovement _citizenMovement => GetComponent<CitizenMovement>();
        private CitizenItemTransport _citizenItemTransport => GetComponent<CitizenItemTransport>();
        private CitizenNeeds _citizenNeeds => GetComponent<CitizenNeeds>();

        private IEnumerable<BaseState<Citizen>> _states;

        public bool IsInBuilding => _citizenMovement.IsInBuilding;
        public Building CurrentBuilding => _citizenMovement.CurrentBuilding;

        public bool IsThirsty => _citizenNeeds.Thirst < 20;
        public bool IsHungry => _citizenNeeds.Hunger < 20;


        private void Awake()
        {
            HomeState = new HomeState();
            SearchWorksiteState = new SearchWorksiteState();
            StoringState = new StoringState();
            TakingFromStorageState = new TakingFromStorageState();
            WanderingState = new WanderingState();
            WorkingState = new WorkingState();

            _stateMachine = new StateMachine<Citizen>(this, HomeState);

            _states = new BaseState<Citizen>[]
            {
                HomeState,
                SearchWorksiteState,
                StoringState,
                TakingFromStorageState,
                WanderingState,
                WorkingState
            };
        }

        private void Start()
        {
            TimeManager.Instance.OnDayChanged += () => _citizenNeeds.Thirst -= 5;
            TimeManager.Instance.OnDayChanged += () => _citizenNeeds.Hunger -= 5;
        }

        private void OnDestroy()
        {
            TimeManager.Instance.OnDayChanged -= () => _citizenNeeds.Thirst -= 5;
            TimeManager.Instance.OnDayChanged -= () => _citizenNeeds.Hunger -= 5;
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

        public void ReplenishThirst(ScriptableItem replenisher)
        {
            _citizenNeeds.Thirst += replenisher.ThirstReplenish;
        }

        public void ReplenishHunger(ScriptableItem replenisher)
        {
            _citizenNeeds.Hunger += replenisher.HungerReplenish;
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
            _stateMachine.SwitchState(newState);
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
        
        public IEnumerator StoreLimitingItemsToMainStorage(IEnumerable<ScriptableItem> items, Storage storageToTakeFrom)
        {
            yield return StartCoroutine(_citizenItemTransport.StoreLimitingItemsToMainStorage(items, storageToTakeFrom));
        }

        public IEnumerator GetItemsFromAvailableStorage(IReadOnlyDictionary<ScriptableItem, int> requiredItems, IReadOnlyDictionary<ScriptableItem, int> availableItems, Storage storageToTakeTo)
        {
            yield return StartCoroutine(_citizenItemTransport.GetItemsFromAvailableStorage(requiredItems, availableItems, storageToTakeTo));
        }
    }
}
