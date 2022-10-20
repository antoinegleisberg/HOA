using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;
    public event Action onOpenUI;
    public event Action onCloseUI;
    public event Action onHoverUI;
    public event Action onMouseLeaveUI;

    void Awake()
    {
        instance = this;
    }

    public void OpenUI()
    {
        if (onOpenUI != null) onOpenUI();
    }

    public void CloseUI()
    {
        if (onCloseUI != null) onCloseUI();
    }

    public void HoverUI()
    {
        if (onHoverUI != null) onHoverUI();
    }

    public void MouseLeaveUI()
    {
        if (onMouseLeaveUI != null) onMouseLeaveUI();
    }
}
