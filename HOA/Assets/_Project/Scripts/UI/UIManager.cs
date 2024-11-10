using UnityEngine;
using antoinegleisberg.UI;
using antoinegleisberg.HOA.EventSystem;
using antoinegleisberg.HOA.Input;
using System.Collections.Generic;
using System.Linq;


namespace antoinegleisberg.HOA.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private Canvas _gameplayUI;
        [SerializeField] private Canvas _buildMenu;
        [SerializeField] private Canvas _previewCanvas;
        [SerializeField] private Canvas _settingsCanvas;
        [SerializeField] private Canvas _objectInfoCanvas;

        [SerializeField] private ObjectInfoManager _objectInfoManager;

        private List<Canvas> _canvases;

        public bool UiIsHovered => _canvases.Any(canvas => canvas.GetComponent<UIHoverDetector>().IsHovered);

        private void Awake()
        {
            Instance = this;

            _canvases = new List<Canvas> { _gameplayUI, _buildMenu, _previewCanvas, _settingsCanvas, _objectInfoCanvas };
            foreach (Canvas canvas in _canvases)
            {
                canvas.gameObject.SetActive(true);
            }

            SetActiveCanvas(_gameplayUI);
        }

        private void Start()
        {
            GameEvents.Instance.OnEnterGameplayState += () => SetActiveCanvas(_gameplayUI);
            GameEvents.Instance.OnResumeGameplay += () => SetActiveCanvas(_gameplayUI);
            GameEvents.Instance.OnEnterPreviewState += () => SetActiveCanvas(_previewCanvas);

            UIEvents.Instance.OnBuildMenuOpen += () => SetActiveCanvas(_buildMenu);
            UIEvents.Instance.OnBuildMenuClose += () => SetActiveCanvas(_gameplayUI);
            UIEvents.Instance.OnCloseObjectInfo += () => SetActiveCanvas(_gameplayUI);

            InputManager.Instance.OnCancel += OnCancel;
            InputManager.Instance.OnMouseClick += OnMouseClick;

            foreach (Canvas canvas in _canvases)
            {
                canvas.GetComponent<UIHoverDetector>().OnHover += (bool _hover) => RaiseHoverUiEvent();
            }
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnEnterGameplayState -= () => SetActiveCanvas(_gameplayUI);
            GameEvents.Instance.OnResumeGameplay -= () => SetActiveCanvas(_gameplayUI);
            GameEvents.Instance.OnEnterPreviewState -= () => SetActiveCanvas(_previewCanvas);

            UIEvents.Instance.OnBuildMenuOpen -= () => SetActiveCanvas(_buildMenu);
            UIEvents.Instance.OnBuildMenuClose -= () => SetActiveCanvas(_gameplayUI);
            UIEvents.Instance.OnCloseObjectInfo -= () => SetActiveCanvas(_gameplayUI);

            InputManager.Instance.OnCancel -= OnCancel;
            InputManager.Instance.OnMouseClick -= OnMouseClick;

            foreach (Canvas canvas in _canvases)
            {
                if (canvas == null)
                {
                    continue;
                }
                if (canvas.TryGetComponent(out UIHoverDetector hoverDetector))
                {
                    hoverDetector.OnHover -= (bool _hover) => RaiseHoverUiEvent();
                }
            }
        }

        private void OnCancel()
        {
            if (_gameplayUI.enabled)
            {
                SetActiveCanvas(_settingsCanvas);
                UIEvents.Instance.OpenSettingsMenu();
            }
            else if (_settingsCanvas.enabled)
            {
                SetActiveCanvas(_gameplayUI);
                UIEvents.Instance.CloseSettingsMenu();
            }
            else
            {
                // Is this enough or do we need to raise events ?
                // SetActiveCanvas(_gameplayUI);
                Debug.Log("Cancel not implemented yet");
            }
        }
        
        private void OnMouseClick()
        {
            if (!(_gameplayUI.enabled || _objectInfoCanvas.enabled))
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.MouseScreenPosition);
            RaycastHit2D raycastHit = Physics2D.GetRayIntersection(ray);
            OnColliderClicked(raycastHit.collider);
        }

        private void OnColliderClicked(Collider2D collider)
        {
            if (UiIsHovered)
            {
                return;
            }

            if (_objectInfoCanvas.enabled)
            {
                _objectInfoManager.OnColliderClicked(collider);
            }
            else if (collider != null)
            {
                SetActiveCanvas(_objectInfoCanvas);
                _objectInfoManager.OnColliderClicked(collider);
                UIEvents.Instance.OpenObjectInfo();
            }
        }

        private void SetActiveCanvas(Canvas canvas)
        {
            Debug.Log("Setting active UI canvas: " + canvas.name);
            _gameplayUI.enabled = false;
            _buildMenu.enabled = false;
            _previewCanvas.enabled = false;
            _settingsCanvas.enabled = false;
            _objectInfoCanvas.enabled = false;
            canvas.enabled = true;
        }

        private void RaiseHoverUiEvent()
        {
            UIEvents.Instance.HoverUi(UiIsHovered);
        }
    }
}
