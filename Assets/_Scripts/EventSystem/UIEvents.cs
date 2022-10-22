using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public static UIEvents instance { get; private set; }

    public event Action<string> onOpenUIMenu;
    public event Action onCloseUIMenu;
    public event Action<string> onPreviewBuilding;
    public event Action onExitPreviewBuilding;
    public event Action onHoverUI;
    public event Action onMouseLeaveUI;

    void Awake() { instance = this; }

    public void OpenUI(string menuName) { if (onOpenUIMenu != null) onOpenUIMenu(menuName); }
    public void CloseUI() { if (onCloseUIMenu != null) onCloseUIMenu(); }
    public void PreviewBuilding(string buildingName) { if (onPreviewBuilding != null) onPreviewBuilding(buildingName); }
    public void ExitPreviewbuilding() { if (onExitPreviewBuilding != null) onExitPreviewBuilding(); }
    public void HoverUI() { if (onHoverUI != null) onHoverUI(); }
    public void MouseLeaveUI() { if (onMouseLeaveUI != null) onMouseLeaveUI(); }
}
