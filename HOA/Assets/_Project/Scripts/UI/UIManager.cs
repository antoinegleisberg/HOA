using UnityEngine;
using antoinegleisberg.UI;
using antoinegleisberg.HOA.EventSystem;


namespace antoinegleisberg.HOA
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private UIHoverDetector _menuButtons;
        [SerializeField] private UIHoverDetector _cancelPreviewButton;
        [SerializeField] private UIHoverDetector _settingsButton;
        [SerializeField] private Canvas _gameplayUI;
        [SerializeField] private Canvas _buildMenu;
        [SerializeField] private Canvas _previewCanvas;
        [SerializeField] private Canvas _settingsCanvas;

        public bool UiIsHovered => _menuButtons.IsHovered || _cancelPreviewButton.IsHovered || _settingsButton.IsHovered;

        private void Awake()
        {
            Instance = this;
            _gameplayUI.gameObject.SetActive(true);
            _buildMenu.gameObject.SetActive(true);
            _previewCanvas.gameObject.SetActive(true);
            _settingsCanvas.gameObject.SetActive(true);
            SetActiveCanvas(_gameplayUI);
        }

        private void Start()
        {
            GameEvents.Instance.OnEnterGameplayState += () => SetActiveCanvas(_gameplayUI);
            GameEvents.Instance.OnResumeGameplay += () => SetActiveCanvas(_gameplayUI);
            GameEvents.Instance.OnEnterUIState += () => SetActiveCanvas(_buildMenu);
            GameEvents.Instance.OnEnterPreviewState += () => SetActiveCanvas(_previewCanvas);

            UIEvents.Instance.OnSettingsMenuOpen += () => SetActiveCanvas(_settingsCanvas);
            UIEvents.Instance.OnSettingsMenuClose += () => SetActiveCanvas(_gameplayUI);

            _menuButtons.OnHover += (bool _hover) => RaiseHoverUiEvent();
            _cancelPreviewButton.OnHover += (bool _hover) => RaiseHoverUiEvent();
            _settingsButton.OnHover += (bool _hover) => RaiseHoverUiEvent();
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnEnterGameplayState -= () => SetActiveCanvas(_gameplayUI);
            GameEvents.Instance.OnResumeGameplay -= () => SetActiveCanvas(_gameplayUI);
            GameEvents.Instance.OnEnterUIState -= () => SetActiveCanvas(_buildMenu);
            GameEvents.Instance.OnEnterPreviewState -= () => SetActiveCanvas(_previewCanvas);

            UIEvents.Instance.OnSettingsMenuOpen -= () => SetActiveCanvas(_settingsCanvas);
            UIEvents.Instance.OnSettingsMenuClose -= () => SetActiveCanvas(_gameplayUI);

            _menuButtons.OnHover -= (bool _hover) => RaiseHoverUiEvent();
            _cancelPreviewButton.OnHover -= (bool _hover) => RaiseHoverUiEvent();
            _settingsButton.OnHover -= (bool _hover) => RaiseHoverUiEvent();
        }

        private void SetActiveCanvas(Canvas canvas)
        {
            _gameplayUI.enabled = false;
            _buildMenu.enabled = false;
            _previewCanvas.enabled = false;
            _settingsCanvas.enabled = false;
            canvas.enabled = true;
        }

        private void RaiseHoverUiEvent()
        {
            UIEvents.Instance.HoverUi(UiIsHovered);
        }
    }
}
