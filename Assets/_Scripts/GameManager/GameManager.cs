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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startupGameState.Init();
        defaultGameState.Init();
        buildPreviewGameState.Init();
        openedUIGameState.Init();

        currentGameState = startupGameState;
        currentGameState.EnterState();
    }

    void Update()
    {
        currentGameState.UpdateState();
    }

    public void SwitchState(BaseGameState newState)
    {
        currentGameState.ExitState();
        currentGameState = newState;
        currentGameState.EnterState();
    }

    public void HandleClickOnTile(Vector3 coordinates)
    {
        Debug.Log(coordinates);
        currentGameState.HandleClickOnTile(coordinates);
    }

    public void HandleNewBuilding(string name)
    {
        SwitchState(buildPreviewGameState);
        buildPreviewGameState.Preview(name);
        Debug.Log(buildPreviewGameState == currentGameState);
    }
}
