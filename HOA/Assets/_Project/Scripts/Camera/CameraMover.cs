using UnityEngine;
using antoinegleisberg.HOA.Input;
using antoinegleisberg.HOA.EventSystem;

namespace antoinegleisberg.HOA.Camera
{
    public class CameraMover : MonoBehaviour
    {
        private Transform _cameraTransform;
        
        [SerializeField] private float _cameraSpeed;
        
        [Range(0, 1)]
        [SerializeField] private float _detectionRange;

        private bool _uiIsHovered;
        private bool _isInGameplayState;
        private bool _isInPreviewState;

        private bool CanMove() => !_uiIsHovered && (_isInGameplayState || _isInPreviewState);

        private void Awake()
        {
            _cameraTransform = transform;
        }

        private void Start()
        {
            GameEvents.Instance.OnEnterGameplayState += () => _isInGameplayState = true;
            GameEvents.Instance.OnExitGameplayState += () => _isInGameplayState = false;

            GameEvents.Instance.OnEnterPreviewState += () => _isInPreviewState = true;
            GameEvents.Instance.OnExitPreviewState += () => _isInPreviewState = false;

            UIEvents.Instance.OnHoverUi += (bool isHovered) => _uiIsHovered = isHovered;
        }

        private void Update()
        {
            if (!CanMove())
            {
                return;
            }

            MoveCamera();
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnEnterGameplayState -= () => _isInGameplayState = true;
            GameEvents.Instance.OnExitGameplayState -= () => _isInGameplayState = false;

            GameEvents.Instance.OnEnterPreviewState -= () => _isInPreviewState = true;
            GameEvents.Instance.OnExitPreviewState -= () => _isInPreviewState = false;

            UIEvents.Instance.OnHoverUi -= (bool isHovered) => _uiIsHovered = isHovered;
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
