using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupGameState : BaseGameState
{
    public override void Init() { }

    public override void EnterState()
    {
        GridManager.instance.GenerateGrid();
        GameManager.instance.SwitchState(GameManager.instance.defaultGameState);
    }

    public override void ExitState() { }

    public override void UpdateState() { }

    public override void HandleClickOnTile(Vector3 coordinates) { }

    public override void OnDestroy() { }
}
