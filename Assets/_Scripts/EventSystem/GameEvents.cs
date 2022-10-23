using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    public event Action onEnterDefaultGameState;
    public event Action onExitDefaultGameState;
    public event Action onEnterPreviewBuildingGameState;
    public event Action onExitPreviewBuildingGameState;
    public event Action onSelectUnitGameState;

    private void Awake()
    {
        instance = this;
    }

    public void EnterDefaultGameState() { if (onEnterDefaultGameState != null) onEnterDefaultGameState(); }
    public void ExitDefaultGameState() { if (onExitDefaultGameState != null) onExitDefaultGameState(); }
    public void EnterPreviewBuildingGameState() { if (onEnterPreviewBuildingGameState != null) onEnterPreviewBuildingGameState(); }
    public void ExitPreviewBuildingGameState() { if (onExitPreviewBuildingGameState != null) onExitPreviewBuildingGameState(); }
    public void EnterSelectUnitGameState () { if (onSelectUnitGameState != null) onSelectUnitGameState(); }
}
