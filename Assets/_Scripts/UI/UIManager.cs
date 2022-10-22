using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] private Canvas _menusCanvas;
    [SerializeField] private Canvas _buttonsCanvas;
    [SerializeField] private Canvas _previewCanvas;

    [SerializeField] private Dictionary<string, GameObject> _menus; // only one can be displayed at a time
    [SerializeField] private Dictionary<string, GameObject> _menuButtons; // are displayed iff DefaultGameState
    [SerializeField] private GameObject _closePreviewButton;

    private GameObject _activeMenu;

    private void Awake() { instance = this; }

    void Start()
    {
        _activeMenu = null;
        InitalizeMenus();
        InitializeMenuButtons();
        SubscribeToEvents();
        _closePreviewButton.SetActive(false);
    }

    private void InitalizeMenus()
    {
        _menus = new Dictionary<string, GameObject>();
        List<GameObject> MenusList = UtilityFunctions.GetChildrenOf(_menusCanvas.gameObject);
        foreach (GameObject menu in MenusList)
        {
            _menus[menu.name] = menu;
            menu.SetActive(false);
        }
    }

    private void InitializeMenuButtons()
    {
        _menuButtons = new Dictionary<string, GameObject>();
        List<GameObject> ButtonsList = UtilityFunctions.GetChildrenOf(_buttonsCanvas.gameObject);
        foreach (GameObject button in ButtonsList) { _menuButtons[button.name] = button; }
    }

    private void SubscribeToEvents()
    {
        UIEvents.instance.onOpenUIMenu += OpenMenu;
        UIEvents.instance.onCloseUIMenu += CloseMenu;
        UIEvents.instance.onPreviewBuilding += EnterPreviewGameState;
        UIEvents.instance.onExitPreviewBuilding += ExitPreviewGameState;
    }

    private void UnsubscribeToEvents()
    {
        UIEvents.instance.onOpenUIMenu -= OpenMenu;
        UIEvents.instance.onCloseUIMenu -= CloseMenu;
        UIEvents.instance.onPreviewBuilding -= EnterPreviewGameState;
        UIEvents.instance.onExitPreviewBuilding -= ExitPreviewGameState;
    }

    private void OpenMenu(string menuName)
    {
        CloseAllMenus();
        GameObject menu = _menus[menuName];
        menu.SetActive(true);
        _activeMenu = menu;
        SetActiveMenuButtons(false);
    }

    private void CloseMenu()
    {
        CloseAllMenus();
        SetActiveMenuButtons(true);
    }

    private void CloseAllMenus()
    {
        _activeMenu = null;
        foreach (GameObject menu in _menus.Values) { menu.SetActive(false); }
    }

    private void SetActiveMenuButtons(bool value)
    {
        foreach (KeyValuePair<string, GameObject> button in _menuButtons) { button.Value.SetActive(value); }
    }

    private void EnterPreviewGameState(string buildingName) { CloseAllMenus(); _closePreviewButton.SetActive(true); }

    private void ExitPreviewGameState()
    {
        _closePreviewButton.SetActive(false);
        SetActiveMenuButtons(true);
    }

    private void OnDestroy() { UnsubscribeToEvents(); }
}
