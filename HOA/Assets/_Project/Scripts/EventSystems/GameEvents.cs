using System;
using UnityEngine;

namespace antoinegleisberg.HOA.EventSystem
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Instance { get; private set; }

        public event Action OnEnterGameplayState;
        public event Action OnExitGameplayState;
        public event Action OnPauseGameplay;
        public event Action OnResumeGameplay;
        public event Action OnEnterUIState;
        public event Action OnExitUIState;
        public event Action OnEnterPreviewState;
        public event Action OnExitPreviewState;

        private void Awake()
        {
            Instance = this;
        }

        public void EnterGameplayState()
        {
            OnEnterGameplayState?.Invoke();
        }

        public void ExitGameplayState()
        {
            OnExitGameplayState?.Invoke();
        }

        public void PauseGameplay()
        {
            OnPauseGameplay?.Invoke();
        }

        public void ResumeGameplay()
        {
            OnResumeGameplay?.Invoke();
        }

        public void EnterUIState()
        {
            OnEnterUIState?.Invoke();
        }

        public void ExitUIState()
        {
            OnExitUIState?.Invoke();
        }

        public void EnterPreviewState()
        {
            OnEnterPreviewState?.Invoke();
        }

        public void ExitPreviewState()
        {
            OnExitPreviewState?.Invoke();
        }
    }
}
