using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Citizen : MonoBehaviour
{
    [SerializeField] public House house;
    [SerializeField] public Worksite worksite;

    [SerializeField] private CitizenBaseState _currentState;
    public BreakState breakState;
    public WorkingState workState;
    public StoringState storeState;
    public SearchWorksiteState searchWorksiteState;
    public TravelingState travelState;
    public SleepingState sleepState;

    public void Init(House house)
    {
        this.house = house;
    }

    private void Awake()
    {
        breakState = new BreakState();
        workState = new WorkingState();
        storeState = new StoringState();
        searchWorksiteState = new SearchWorksiteState();
        travelState = new TravelingState();
        sleepState = new SleepingState();
    }

    void Start()
    {
        worksite = null;

        breakState.Init(this);
        workState.Init(this);
        storeState.Init(this);
        searchWorksiteState.Init(this);
        travelState.Init(this);

        _currentState = breakState;
        _currentState.EnterState(this);

        SubscribeToEvents();
    }

    void Update()
    {
        _currentState.UpdateState(this);
    }

    public void SwitchState(CitizenBaseState newState)
    {
        _currentState.ExitState(this);
        Debug.Log("Switching state from " + _currentState + " to " + newState);
        _currentState = newState;
        _currentState.EnterState(this);
    }

    private void SwitchToBreakState()
    {
        SwitchState(breakState);
    }

    private void OnGoToWork()
    {
        travelState.SetDestination(Destination.Worksite);
        SwitchState(travelState);
    }

    private void SwitchToSleepState() => SwitchState(sleepState);

    private void SubscribeToEvents()
    {
        TimeEvents.instance.onMorning += SwitchToBreakState;
        TimeEvents.instance.onMorningWork += OnGoToWork;
        TimeEvents.instance.onMidday += SwitchToBreakState;
        TimeEvents.instance.onAfternoonWork += OnGoToWork;
        TimeEvents.instance.onEvening += SwitchToBreakState;
        TimeEvents.instance.onNight += SwitchToSleepState;
    }

    private void UnsubscribeFromEvents()
    {
        TimeEvents.instance.onMorning -= SwitchToBreakState;
        TimeEvents.instance.onMorningWork -= OnGoToWork;
        TimeEvents.instance.onMidday -= SwitchToBreakState;
        TimeEvents.instance.onAfternoonWork -= OnGoToWork;
        TimeEvents.instance.onEvening -= SwitchToBreakState;
        TimeEvents.instance.onNight -= SwitchToSleepState;
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
}