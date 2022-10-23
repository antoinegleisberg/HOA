using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }
    public Vector2 mouseScreenPosition { get; private set; }
    public Vector3 mouseWorldPosition { get; private set; }

    private void Awake() { instance = this; }

    private void Start() { UpdateMousePosition(); }

    void Update() { UpdateMousePosition(); }

    private void UpdateMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mouseScreenPosition = new Vector2(mousePosition.x, mousePosition.y);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        mouseWorldPosition = ray.origin - ray.direction * ray.origin.z / ray.direction.z;
    }
}
