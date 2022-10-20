using UnityEngine;

public abstract class BaseGameState
{
    public abstract void Init();

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void HandleClickOnTile(Vector3 coordinates);
}
