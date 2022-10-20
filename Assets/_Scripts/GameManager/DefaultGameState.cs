using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGameState : BaseGameState
{
    public override void Init() { }

    public override void EnterState()
    {
        // Time.timeScale = 1;
    }

    public override void ExitState()
    {
        // Time.timeScale = 0;
    }
    
    public override void UpdateState() { }

    public override void HandleClickOnTile(Vector3 coordinates)
    {
        
    }
}
