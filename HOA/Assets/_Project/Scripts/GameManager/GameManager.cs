using System;
using UnityEditor;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public enum GameState
    {
        Gameplay,
        UI,
        Preview
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

            InputManager.Instance.OnMouseClick += OnMouseClick;
        }


        private void OnDestroy()
        {
            UIEvents.Instance.OnBuildMenuOpen -= () => SwitchState(GameState.UI);
            UIEvents.Instance.OnBuildMenuClose -= () => SwitchState(GameState.Gameplay);
            UIEvents.Instance.OnBuildBuildingSelected -= OnBuildingSelected;
            UIEvents.Instance.OnCancelPreview -= OnCancelPreview;

            InputManager.Instance.OnMouseClick -= OnMouseClick;
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
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));

                Vector3 localPosition = GridManager.Instance.Grid.WorldToLocal(worldPosition);

                Vector3 interpolatedCellPosition = GridManager.Instance.Grid.LocalToCellInterpolated(localPosition);
                
                BuildingsBuilder.Instance.BuildBuilding(PreviewManager.Instance.PreviewBuilding, interpolatedCellPosition);
                
                PreviewManager.Instance.CancelPreview();
                SwitchState(GameState.Gameplay);
            }
        }
    }
}
