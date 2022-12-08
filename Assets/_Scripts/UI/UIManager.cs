using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    public bool isHoveringUI { get; private set; }
    public bool UIisOpened { get; private set; }

    [SerializeField] private Canvas _menusCanvas;
    [SerializeField] private Canvas _buttonsCanvas;
    [SerializeField] private Canvas _previewCanvas;

    [SerializeField] private Dictionary<string, GameObject> _menus;
    [SerializeField] private Dictionary<string, GameObject> _menuButtons;
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
        isHoveringUI = false;
        UIisOpened = false;
    }

    private void InitalizeMenus()
    {
        _menus = new Dictionary<string, GameObject>();
        List<GameObject> MenusList = UtilityFunctions.GetChildrenOf(_menusCanvas.gameObject);
        foreach (GameObject menu in MenusList)
        {
            _menus[menu.name] = menu;
            menu.SetActive(false);
            foreach (RectTransform rT in menu.GetComponentInChildren<RectTransform>())
            {
                Debug.Log($"{rT.gameObject}");
            }
        }
    }

    private void InitializeMenuButtons()
    {
        _menuButtons = new Dictionary<string, GameObject>();
        List<GameObject> ButtonsList = UtilityFunctions.GetChildrenOf(_buttonsCanvas.gameObject);
        foreach (GameObject button in ButtonsList) { _menuButtons[button.name] = button; button.SetActive(true); }
    }

    private void SubscribeToEvents()
    {
        UIEvents.instance.onOpenUIMenu += OpenMenu;
        UIEvents.instance.onCloseUIMenu += CloseMenu;
        UIEvents.instance.onHoverUI += SetMouseHoveringUI;
        UIEvents.instance.onMouseLeaveUI += SetMouseLeavingUI;
        UIEvents.instance.onEnterPreview += EnterPreviewGameState;
        UIEvents.instance.onExitPreview += ExitPreviewGameState;
        GameEvents.instance.onEnterDefaultGameState += EnterDefaultGameState;
        GameEvents.instance.onExitDefaultGameState += ExitDefaultGameState;
    }

    private void UnsubscribeToEvents()
    {
        UIEvents.instance.onOpenUIMenu -= OpenMenu;
        UIEvents.instance.onCloseUIMenu -= CloseMenu;
        UIEvents.instance.onHoverUI -= SetMouseHoveringUI;
        UIEvents.instance.onMouseLeaveUI -= SetMouseLeavingUI;
        UIEvents.instance.onEnterPreview -= EnterPreviewGameState;
        UIEvents.instance.onExitPreview -= ExitPreviewGameState;
        GameEvents.instance.onEnterDefaultGameState -= EnterDefaultGameState;
        GameEvents.instance.onExitDefaultGameState -= ExitDefaultGameState;
    }

    private void OpenMenu(string menuName)
    {
        CloseAllMenus();
        GameObject menu = _menus[menuName];
        menu.SetActive(true);
        _activeMenu = menu;
        UIisOpened = true;
    }

    private void CloseMenu()
    {
        CloseAllMenus();
        SetActiveMenuButtons(true);
        UIisOpened = false; isHoveringUI = false;
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

    private void EnterPreviewGameState(string buildingName) { 
        CloseAllMenus();
        _closePreviewButton.SetActive(true);
        UIisOpened = false; isHoveringUI = false;
    }

    private void ExitPreviewGameState() { _closePreviewButton.SetActive(false); isHoveringUI = false; }

    private void EnterDefaultGameState() { SetActiveMenuButtons(true); }

    private void ExitDefaultGameState() { SetActiveMenuButtons(false); }

    private void SetMouseHoveringUI() { isHoveringUI = true; }

    private void SetMouseLeavingUI() { isHoveringUI = false; }

    private void OnDestroy() { UnsubscribeToEvents(); }
}
