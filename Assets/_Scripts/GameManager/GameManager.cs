using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] BaseGameState currentGameState;
    public StartupGameState startupGameState = new StartupGameState();
    public DefaultGameState defaultGameState = new DefaultGameState();
    public BuildPreviewGameState buildPreviewGameState = new BuildPreviewGameState();
    public OpenedUIGameState openedUIGameState = new OpenedUIGameState();

    private void Awake() { instance = this; }

    void Start()
    {
        startupGameState.Init();
        defaultGameState.Init();
        buildPreviewGameState.Init();
        openedUIGameState.Init();

        SubscribeToEvents();

        currentGameState = startupGameState;
        currentGameState.EnterState();
    }

    void Update() { currentGameState.UpdateState(); }

    public void SwitchState(BaseGameState newState)
    {
        currentGameState.ExitState();
        // Debug.Log($"Entering {newState} state");
        currentGameState = newState;
        currentGameState.EnterState();
    }

    public void HandleClickOnTile(Vector3 coordinates) { currentGameState.HandleClickOnTile(coordinates); }

    private void SubscribeToEvents()
    {
        UIEvents.instance.onOpenUIMenu += SwitchToOpenedUIGameState;
        UIEvents.instance.onCloseUIMenu += SwitchToDefaultGameState;
        UIEvents.instance.onEnterPreview += SwitchToPreviewBuildingGameState;
        UIEvents.instance.onExitPreview += SwitchToDefaultGameState;
    }

    private void UnsubscribeToEvents()
    {
        UIEvents.instance.onOpenUIMenu -= SwitchToOpenedUIGameState;
        UIEvents.instance.onCloseUIMenu -= SwitchToDefaultGameState;
        UIEvents.instance.onEnterPreview -= SwitchToPreviewBuildingGameState;
        UIEvents.instance.onExitPreview -= SwitchToDefaultGameState;
    }

    private void SwitchToOpenedUIGameState(string menuName) { SwitchState(openedUIGameState); }
    private void SwitchToDefaultGameState() { SwitchState(defaultGameState); }
    private void SwitchToPreviewBuildingGameState(string buildingName)
    {
        SwitchState(buildPreviewGameState);
        buildPreviewGameState.Preview(buildingName);
    }

    private void OnDestroy() { UnsubscribeToEvents(); }
}
