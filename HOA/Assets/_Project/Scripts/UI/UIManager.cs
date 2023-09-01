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

        public bool UiIsHovered => _menuButtons.IsHovered || _cancelPreviewButton.IsHovered;

        private void Awake()
        {
            Instance = this;
            ActivateGameplayUI();
        }

        private void Start()
        {
            GameEvents.Instance.OnEnterGameplayState += OnEnterGameplayState;
            GameEvents.Instance.OnEnterUIState += OnEnterUIState;
            GameEvents.Instance.OnEnterPreviewState += OnEnterPreviewState;
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnEnterGameplayState -= OnEnterGameplayState;
            GameEvents.Instance.OnEnterUIState -= OnEnterUIState;
            GameEvents.Instance.OnEnterPreviewState -= OnEnterPreviewState;
        }

        private void OnEnterGameplayState()
        {
            ActivateGameplayUI();
        }

        private void OnEnterUIState()
        {
            ActivateBuildMenu();
        }

        private void OnEnterPreviewState()
        {
            _buildMenu.enabled = false;
            _gameplayUI.enabled = false;
            _previewCanvas.enabled = true;
        }

        private void ActivateGameplayUI()
        {
            _buildMenu.enabled = false;
            _gameplayUI.enabled = true;
            _previewCanvas.enabled = false;
        }

        private void ActivateBuildMenu()
        {
            _gameplayUI.enabled = false;
            _buildMenu.enabled = true;
            _previewCanvas.enabled = false;
        }
    }
}
