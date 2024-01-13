using UnityEngine;


namespace antoinegleisberg.HOA
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;

        [SerializeField] private float _cameraSpeed;
        [Range(0, 1)]
        [SerializeField] private float _detectionRange;

        void Update()
        {
            if (GameManager.Instance.CurrentState == GameState.UI)
            {
                return;
            }
            
            if (UIManager.Instance.UiIsHovered)
            {
                return;
            }

            MoveCamera();
        }

        private void MoveCamera()
        {
            Vector2 mousePos = InputManager.Instance.MouseRelativeScreenPosition;
            
            Vector3 cameraOffset = Vector3.zero;
            if (0 < mousePos.x && mousePos.x < _detectionRange)
            {
                cameraOffset += Vector3.left;
            }
            if (1 - _detectionRange < mousePos.x && mousePos.x < 1)
            {
                cameraOffset += Vector3.right;
            }
            if (0 < mousePos.y && mousePos.y < _detectionRange)
            {
                cameraOffset += Vector3.down;
            }
            if (1 - _detectionRange < mousePos.y && mousePos.y < 1)
            {
                cameraOffset += Vector3.up;
            }

            cameraOffset.Normalize();
            cameraOffset *= Time.deltaTime * _cameraSpeed;
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _cameraTransform.position + cameraOffset, 0.5f);
        }
    }
}
