using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Camera _cam;
    private Transform _cameraPosition;

    [SerializeField] private bool _canMove;
    private bool _hoveringUI;
    private bool _openedUI;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float detectionRange;

    private void Start()
    {
        SubscribeToEvents();
        _canMove = true;
        _hoveringUI = false;
        _openedUI = false;
        _cam = GetComponent<Camera>();
        _cameraPosition = GetComponent<Transform>();
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

    void Update()
    {
        _canMove = !_hoveringUI && !_openedUI;
        if (_canMove) MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 cameraShift = new Vector3(0, 0, 0);
        Vector3 right = new Vector3(1, -1, 0).normalized;
        Vector3 up = new Vector3(1, 1, 0).normalized;
        if (0 < mousePos.x && mousePos.x < detectionRange)
        {
            cameraShift += -right * (detectionRange - mousePos.x);
        }
        if (0 < mousePos.y && mousePos.y < detectionRange)
        {
            cameraShift += -up * (detectionRange - mousePos.y);
        }
        if (Screen.width > mousePos.x && mousePos.x > Screen.width - detectionRange)
        {
            cameraShift += right * (-Screen.width + mousePos.x + detectionRange);
        }
        if (Screen.height > mousePos.y && mousePos.y > Screen.height - detectionRange)
        {
            cameraShift += up * (-Screen.height + mousePos.y + detectionRange);
        }
        cameraShift *= cameraSpeed * Time.deltaTime / detectionRange;
        _cameraPosition.position = _cameraPosition.position + cameraShift;
    }

    private void HandleOpeningUI()
    {
        _openedUI = true;
    }

    private void HandleClosingUI()
    {
        _openedUI = false;
    }

    private void HandleMouseHoveringUI()
    {
        _hoveringUI = true;
    }

    private void HandleMouseLeavingUI()
    {
        _hoveringUI = false;
    }

    public void LockCamera()
    {
        _canMove = false;
    }

    public void UnlockCamera()
    {
        _canMove = true;
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
