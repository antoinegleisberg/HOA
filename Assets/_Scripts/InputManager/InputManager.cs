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
        Vector3 mousePosition = Input.mousePosition;
        mouseScreenPosition = new Vector2(mousePosition.x, mousePosition.y);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        mouseWorldPosition = ray.origin - ray.direction * ray.origin.z / ray.direction.z;
        isHoveringUI = false;
        UIisOpened = false;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mouseScreenPosition = new Vector2(mousePosition.x, mousePosition.y);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        mouseWorldPosition = ray.origin - ray.direction * ray.origin.z / ray.direction.z;
    }
    private void SubscribeToEvents()
    {
        GameEvents.instance.onOpenUI += HandleOpeningUI;
        GameEvents.instance.onCloseUI += HandleClosingUI;
        GameEvents.instance.onHoverUI += HandleMouseHoveringUI;
        GameEvents.instance.onMouseLeaveUI += HandleMouseLeavingUI;
    }

    private void UnsubscribeToEvents()
    {
        GameEvents.instance.onOpenUI -= HandleOpeningUI;
        GameEvents.instance.onCloseUI -= HandleClosingUI;
        GameEvents.instance.onHoverUI -= HandleMouseHoveringUI;
        GameEvents.instance.onMouseLeaveUI -= HandleMouseLeavingUI;
    }

    private void HandleOpeningUI()
    {
        UIisOpened = true;
    }

    private void HandleClosingUI()
    {
        UIisOpened = false;
    }

    private void HandleMouseHoveringUI()
    {
        isHoveringUI = true;
    }

    private void HandleMouseLeavingUI()
    {
        isHoveringUI = false;
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
