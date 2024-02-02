using System;
using UnityEditor;
using UnityEngine;

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

        private GameState _currentState;

        public GameState CurrentState => _currentState;

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

            InputManager.Instance.OnCancel -= OnCancel;
            InputManager.Instance.OnMouseClick -= OnMouseClick;
        }

        public void SaveGame()
        {
            GameLoader.Instance.SaveGame();
        }

        private void SwitchState(GameState newState)
        {
            _currentState = newState;
            switch (_currentState)
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

                if (UIManager.Instance.UiIsHovered)
                {
                    return;
                }

                Vector2 mousePos = InputManager.Instance.MouseScreenPosition;
                Vector3 worldPos = GridManager.Instance.MouseToWorldPosition(mousePos);
                BuildingsBuilder.Instance.BuildBuilding(PreviewManager.Instance.PreviewBuilding, worldPos);
                
                PreviewManager.Instance.CancelPreview();
                SwitchState(GameState.Gameplay);
            }
        }

        private void OnCancel()
        {
            if (_currentState == GameState.Gameplay)
            {
                UIEvents.Instance.OpenSettingsMenu();
            }
            else
            {
                UIEvents.Instance.CloseSettingsMenu();
            }
        }
    }
}
