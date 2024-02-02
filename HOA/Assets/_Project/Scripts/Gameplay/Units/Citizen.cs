using antoinegleisberg.StateMachine;
using System.Collections;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class Citizen : MonoBehaviour
    {
        [SerializeField] private float _speed;

        [field: SerializeField] public float WanderingDistance { get; private set; }

        [field: SerializeField] public float TimeAtWork { get; private set; }
        [field: SerializeField] public float TimeAtHome { get; private set; }
        [field: SerializeField] public float TimeWandering { get; private set; }

        [field:SerializeField] public House Home { get; private set; }
        [field: SerializeField] public Workplace Workplace { get; private set; }

        
        private StateMachine<Citizen> _stateMachine;
        public HomeState HomeState { get; private set; }
        public SearchWorksiteState SearchWorksiteState { get; private set; }
        public WorkingState WorkingState { get; private set; }
        public StoringState StoringState { get; private set; }
        public TakingFromStorageState TakingFromStorageState { get; private set; }
        public WanderingState WanderingState { get; private set; }


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
            house.Residents.Add(this);
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
            Transform t = transform;
            while (t.position != targetPosition)
            {
                t.position = Vector3.MoveTowards(t.position, targetPosition, _speed * Time.deltaTime);
                yield return null;
            }
        }
        
        public IEnumerator MoveToBuilding(Building target)
        {
            yield return StartCoroutine(MoveToPosition(target.transform.position));
        }
    }
}
