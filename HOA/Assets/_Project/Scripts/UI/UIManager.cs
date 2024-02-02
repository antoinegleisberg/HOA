using UnityEngine;
using antoinegleisberg.UI;


namespace antoinegleisberg.HOA
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private UIHoverDetector _menuButtons;
        [SerializeField] private UIHoverDetector _cancelPreviewButton;
        [SerializeField] private Canvas _gameplayUI;
        [SerializeField] private Canvas _buildMenu;
        [SerializeField] private Canvas _previewCanvas;
        [SerializeField] private Canvas _settingsCanvas;

        public bool UiIsHovered => _menuButtons.IsHovered || _cancelPreviewButton.IsHovered;

        private void Awake()
        {
            Instance = this;
            _gameplayUI.gameObject.SetActive(true);
            _buildMenu.gameObject.SetActive(true);
            _previewCanvas.gameObject.SetActive(true);
            _settingsCanvas.gameObject.SetActive(true);
            ActivateGameplayUI();
        }

        private void Start()
        {
            GameEvents.Instance.OnEnterGameplayState += OnEnterGameplayState;
            GameEvents.Instance.OnPauseGameplay += OnOpenSettingsMenu;
            GameEvents.Instance.OnResumeGameplay += OnEnterGameplayState;
            GameEvents.Instance.OnEnterUIState += OnEnterUIState;
            GameEvents.Instance.OnEnterPreviewState += OnEnterPreviewState;

            UIEvents.Instance.OnSettingsMenuClose += OnEnterGameplayState;
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnEnterGameplayState -= OnEnterGameplayState;
            GameEvents.Instance.OnPauseGameplay -= OnOpenSettingsMenu;
            GameEvents.Instance.OnResumeGameplay -= OnEnterGameplayState;
            GameEvents.Instance.OnEnterUIState -= OnEnterUIState;
            GameEvents.Instance.OnEnterPreviewState -= OnEnterPreviewState;

            UIEvents.Instance.OnSettingsMenuClose -= OnEnterGameplayState;
        }

        private void OnEnterGameplayState()
        {
            ActivateGameplayUI();
        }

        private void OnEnterUIState()
        {
            ActivateBuildMenu();
        }

        private void OnOpenSettingsMenu()
        {
            ActivateSettingsMenu();
        }

        private void OnEnterPreviewState()
        {
            _buildMenu.enabled = false;
            _gameplayUI.enabled = false;
            _previewCanvas.enabled = true;
            _settingsCanvas.enabled = false;
        }

        private void ActivateGameplayUI()
        {
            _buildMenu.enabled = false;
            _gameplayUI.enabled = true;
            _previewCanvas.enabled = false;
            _settingsCanvas.enabled = false;
        }

        private void ActivateBuildMenu()
        {
            _gameplayUI.enabled = false;
            _buildMenu.enabled = true;
            _previewCanvas.enabled = false;
            _settingsCanvas.enabled = false;
        }

        private void ActivateSettingsMenu()
        {
            _gameplayUI.enabled = false;
            _buildMenu.enabled = false;
            _previewCanvas.enabled = false;
            _settingsCanvas.enabled = true;
        }
    }
}
