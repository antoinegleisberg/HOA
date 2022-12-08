using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public static UIEvents instance { get; private set; }

    public event Action<string> onOpenUIMenu;
    public event Action onCloseUIMenu;
    public event Action onHoverUI;
    public event Action onMouseLeaveUI;
    public event Action<string> onEnterPreview;
    public event Action onExitPreview;

    void Awake() { instance = this; }

    public void OpenUI(string menuName) { if (onOpenUIMenu != null) onOpenUIMenu(menuName); }
    public void CloseUI() { if (onCloseUIMenu != null) onCloseUIMenu(); }
    public void HoverUI() { if (onHoverUI != null) onHoverUI(); }
    public void MouseLeaveUI() { if (onMouseLeaveUI != null) onMouseLeaveUI(); }
    public void EnterPreview(string buildingName) { if (onEnterPreview != null) onEnterPreview(buildingName); }
    public void ExitPreview() { if (onExitPreview != null) onExitPreview(); }
}
