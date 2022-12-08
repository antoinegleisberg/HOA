using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGameState : BaseGameState
{
    public override void Init() { }

    public override void EnterState() { GameEvents.instance.EnterDefaultGameState(); }

    public override void ExitState() { GameEvents.instance.ExitDefaultGameState(); }
    
    public override void UpdateState() { }

    public override void HandleClickOnTile(Vector3 coordinates) { }

    public override void OnDestroy() { }
}
