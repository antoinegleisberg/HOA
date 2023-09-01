using antoinegleisberg.StateMachine;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class Citizen : MonoBehaviour
    {
        public House House { get; private set; }
        public Workplace Workplace { get; private set; }

        
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
            WorkingState = new WorkingState();
            StoringState = new StoringState();
            TakingFromStorageState = new TakingFromStorageState();
            WanderingState = new WanderingState();

            _stateMachine = new StateMachine<Citizen>(this, HomeState);
            HomeState.EnterState(this);
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        public void SetHouse(House house)
        {
            House = house;
        }

        public void SetWorkplace(Workplace workplace)
        {
            Workplace = workplace;
        }

        public void SwitchState(BaseState<Citizen> newState)
        {
            Debug.Log("Switching to state " + newState.GetType().ToString());
            _stateMachine.SwitchState(newState);
        }

        public void MoveToBuilding(Building target)
        {

        }
    }
}
