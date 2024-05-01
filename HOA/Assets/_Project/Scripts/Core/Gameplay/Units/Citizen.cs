using antoinegleisberg.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(CitizenMovement), typeof(CitizenItemTransport))]
    public class Citizen : MonoBehaviour
    {
        [field: SerializeField] public float WanderingDistance { get; private set; }

        [field: SerializeField] public float TimeAtWork { get; private set; }
        [field: SerializeField] public float TimeAtHome { get; private set; }
        [field: SerializeField] public float TimeWandering { get; private set; }

        [field: SerializeField] public House Home { get; private set; }
        [field: SerializeField] public Workplace Workplace { get; private set; }


        private StateMachine<Citizen> _stateMachine;
        public HomeState HomeState { get; private set; }
        public SearchWorksiteState SearchWorksiteState { get; private set; }
        public WorkingState WorkingState { get; private set; }
        public StoringState StoringState { get; private set; }
        public TakingFromStorageState TakingFromStorageState { get; private set; }
        public WanderingState WanderingState { get; private set; }

        private CitizenMovement _citizenMovement => GetComponent<CitizenMovement>();
        private CitizenItemTransport _citizenItemTransport => GetComponent<CitizenItemTransport>();

        private void Awake()
        {
            HomeState = new HomeState();
            SearchWorksiteState = new SearchWorksiteState();
            StoringState = new StoringState();
            TakingFromStorageState = new TakingFromStorageState();
            WanderingState = new WanderingState();
            WorkingState = new WorkingState();

            _stateMachine = new StateMachine<Citizen>(this, HomeState);
        }

        public void ClaimHouse(House house)
        {
            Home = house;
            house.AddResident(this);
        }

        public void ClaimWorkplace(Workplace workplace)
        {
            Workplace = workplace;
            workplace.AddWorker(this);
        }

        public void SwitchState(BaseState<Citizen> newState)
        {
            Debug.Log("Switching to state " + newState.GetType().ToString());
            _stateMachine.SwitchState(newState);
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
