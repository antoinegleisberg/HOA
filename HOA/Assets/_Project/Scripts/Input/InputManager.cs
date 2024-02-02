using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace antoinegleisberg.HOA
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public Vector2 MouseScreenPosition { get; private set; }
        public Vector2 MouseRelativeScreenPosition {get; private set; }

        public event Action OnMouseClick;
        public event Action OnCancel;

        [SerializeField] private PlayerInput _playerInput;

        private void Awake()
        {
            Instance = this;
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnMouseClick?.Invoke();
            }
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            Vector2 mouseScreenPosition = context.ReadValue<Vector2>();

            MouseScreenPosition = mouseScreenPosition;
            MouseRelativeScreenPosition = new Vector2(mouseScreenPosition.x / Screen.currentResolution.width, mouseScreenPosition.y / Screen.currentResolution.height);
        }

        public void OnCancelPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnCancel?.Invoke();
            }
        }
    }
}
