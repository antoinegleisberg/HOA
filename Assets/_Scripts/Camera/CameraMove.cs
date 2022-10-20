using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform _cameraPosition;

    [SerializeField] private bool _canMove;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float detectionRange;

    private void Start()
    {
        _cameraPosition = GetComponent<Transform>();
    }

    void Update()
    {
        _canMove = !InputManager.instance.isHoveringUI && !InputManager.instance.UIisOpened;
        if (_canMove) MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 mousePos = InputManager.instance.mouseScreenPosition;
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
}
