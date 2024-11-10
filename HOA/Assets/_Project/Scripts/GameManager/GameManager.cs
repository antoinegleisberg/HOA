using UnityEngine;
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
        public GameState CurrentState { get; private set; }

        private void Start()
        {
            SwitchState(GameState.Gameplay);
            
            UIEvents.Instance.OnBuildMenuOpen += () => SwitchState(GameState.UI);
            UIEvents.Instance.OnBuildMenuClose += () => SwitchState(GameState.Gameplay);
            UIEvents.Instance.OnBuildBuildingSelected += (string buildingName) => SwitchState(GameState.Preview);
            UIEvents.Instance.OnCancelPreview += () => SwitchState(GameState.Gameplay);
            UIEvents.Instance.OnSettingsMenuOpen += () => SwitchState(GameState.Paused);
            UIEvents.Instance.OnSettingsMenuClose += () => SwitchState(GameState.Gameplay);
            UIEvents.Instance.OnOpenObjectInfo += () => SwitchState(GameState.UI);
            UIEvents.Instance.OnCloseObjectInfo += () => SwitchState(GameState.Gameplay);
        }


        private void OnDestroy()
        {
            UIEvents.Instance.OnBuildMenuOpen -= () => SwitchState(GameState.UI);
            UIEvents.Instance.OnBuildMenuClose -= () => SwitchState(GameState.Gameplay);
            UIEvents.Instance.OnBuildBuildingSelected -= (string buildingName) => SwitchState(GameState.Preview);
            UIEvents.Instance.OnCancelPreview -= () => SwitchState(GameState.Gameplay);
            UIEvents.Instance.OnSettingsMenuOpen -= () => SwitchState(GameState.Paused);
            UIEvents.Instance.OnSettingsMenuClose -= () => SwitchState(GameState.Gameplay);
            UIEvents.Instance.OnOpenObjectInfo -= () => SwitchState(GameState.UI);
            UIEvents.Instance.OnCloseObjectInfo -= () => SwitchState(GameState.Gameplay);
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
    }
}
