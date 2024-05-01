using UnityEngine;
using antoinegleisberg.HOA.Input;
using antoinegleisberg.HOA.EventSystem;

namespace antoinegleisberg.HOA
{
    public enum GameState
    {
        Gameplay,
        UI,
        Preview,
        Paused
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState CurrentState { get; private set; }

        private bool _uiIsHovered;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SwitchState(GameState.Gameplay);
            
            UIEvents.Instance.OnBuildMenuOpen += () => SwitchState(GameState.UI);
            UIEvents.Instance.OnBuildMenuClose += () => SwitchState(GameState.Gameplay);
            UIEvents.Instance.OnBuildBuildingSelected += OnBuildingSelected;
            UIEvents.Instance.OnCancelPreview += OnCancelPreview;
            UIEvents.Instance.OnSettingsMenuOpen += () => SwitchState(GameState.Paused);
            UIEvents.Instance.OnSettingsMenuClose += () => SwitchState(GameState.Gameplay);
            UIEvents.Instance.OnHoverUi += OnHoverUi;

            InputManager.Instance.OnCancel += OnCancel;
            InputManager.Instance.OnMouseClick += OnMouseClick;
        }


        private void OnDestroy()
        {
            UIEvents.Instance.OnBuildMenuOpen -= () => SwitchState(GameState.UI);
            UIEvents.Instance.OnBuildMenuClose -= () => SwitchState(GameState.Gameplay);
            UIEvents.Instance.OnBuildBuildingSelected -= OnBuildingSelected;
            UIEvents.Instance.OnCancelPreview -= OnCancelPreview;
            UIEvents.Instance.OnSettingsMenuOpen -= () => SwitchState(GameState.Paused);
            UIEvents.Instance.OnSettingsMenuClose -= () => SwitchState(GameState.Gameplay);
            UIEvents.Instance.OnHoverUi -= OnHoverUi;

            InputManager.Instance.OnCancel -= OnCancel;
            InputManager.Instance.OnMouseClick -= OnMouseClick;
        }

        private void SwitchState(GameState newState)
        {
            switch (CurrentState)
            {
                case GameState.Gameplay:
                    GameEvents.Instance.ExitGameplayState();
                    break;
                case GameState.UI:
                    GameEvents.Instance.ExitUIState();
                    break;
                case GameState.Preview:
                    GameEvents.Instance.ExitPreviewState();
                    break;
                case GameState.Paused:
                    GameEvents.Instance.ResumeGameplay();
                    break;
            }

            CurrentState = newState;
            
            switch (CurrentState)
            {
                case GameState.Gameplay:
                    GameEvents.Instance.EnterGameplayState();
                    break;
                case GameState.UI:
                    GameEvents.Instance.EnterUIState();
                    break;
                case GameState.Preview:
                    GameEvents.Instance.EnterPreviewState();
                    break;
                case GameState.Paused:
                    GameEvents.Instance.PauseGameplay();
                    break;
            }
        }

        private void OnHoverUi(bool isHovered)
        {
            _uiIsHovered = isHovered;
        }

        private void OnBuildingSelected(string name)
        {
            SwitchState(GameState.Preview);
            PreviewManager.Instance.StartPreview(name);
        }

        private void OnCancelPreview()
        {
            PreviewManager.Instance.CancelPreview();
            SwitchState(GameState.Gameplay);
        }

        private void OnMouseClick()
        {
            if (CurrentState == GameState.Preview)
            {
                if (!PreviewManager.Instance.CurrentPositionIsValid)
                {
                    return;
                }

                if (_uiIsHovered)
                {
                    return;
                }

                Vector2 mousePos = InputManager.Instance.MouseScreenPosition;
                Vector3 worldPos = GridManager.Instance.MouseToWorldPosition(mousePos);
                BuildingsBuilder.Instance.BuildBuildingBuildsite(PreviewManager.Instance.PreviewBuilding, worldPos);
                
                PreviewManager.Instance.CancelPreview();
                SwitchState(GameState.Gameplay);
            }
        }

        private void OnCancel()
        {
            if (CurrentState == GameState.Gameplay)
            {
                UIEvents.Instance.OpenSettingsMenu();
            }
            else if (CurrentState == GameState.Paused)
            {
                UIEvents.Instance.CloseSettingsMenu();
            }
        }
    }
}
