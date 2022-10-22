using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }
    public Vector2 mouseScreenPosition { get; private set; }
    public Vector3 mouseWorldPosition { get; private set; }
    public bool isHoveringUI { get; private set; }
    public bool UIisOpened { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SubscribeToEvents();
        UpdateMousePosition();
        isHoveringUI = false;
        UIisOpened = false;
    }

    void Update()
    {
        UpdateMousePosition();
    }

    private void UpdateMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mouseScreenPosition = new Vector2(mousePosition.x, mousePosition.y);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        mouseWorldPosition = ray.origin - ray.direction * ray.origin.z / ray.direction.z;
    }

    private void SubscribeToEvents()
    {
        UIEvents.instance.onOpenUIMenu += SetOpenUI;
        UIEvents.instance.onCloseUIMenu += SetClosedUI;
        UIEvents.instance.onHoverUI += SetMouseHoveringUI;
        UIEvents.instance.onMouseLeaveUI += SetMouseLeavingUI;
        UIEvents.instance.onPreviewBuilding += SetClosedUI;
        UIEvents.instance.onExitPreviewBuilding += SetMouseLeavingUI;
    }

    private void UnsubscribeToEvents()
    {
        UIEvents.instance.onOpenUIMenu -= SetOpenUI;
        UIEvents.instance.onCloseUIMenu -= SetClosedUI;
        UIEvents.instance.onHoverUI -= SetMouseHoveringUI;
        UIEvents.instance.onMouseLeaveUI -= SetMouseLeavingUI;
        UIEvents.instance.onPreviewBuilding -= SetClosedUI;
        UIEvents.instance.onExitPreviewBuilding -= SetMouseLeavingUI;
    }

    private void SetOpenUI(string menuName) { UIisOpened = true; }

    private void SetClosedUI() { UIisOpened = false; isHoveringUI = false; }

    private void SetMouseHoveringUI() { isHoveringUI = true; }

    private void SetMouseLeavingUI() { isHoveringUI = false; }

    private void SetClosedUI(string buildingName) { SetClosedUI(); }

    private void OnDestroy() { UnsubscribeToEvents(); }
}
