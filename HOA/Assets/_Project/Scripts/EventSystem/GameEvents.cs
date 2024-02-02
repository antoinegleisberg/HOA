using System;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Instance { get; private set; }

        public event Action OnEnterGameplayState;
        public event Action OnPauseGameplay;
        public event Action OnResumeGameplay;
        public event Action OnEnterUIState;
        public event Action OnEnterPreviewState;

        private void Awake()
        {
            Instance = this;
        }

        public void EnterGameplayState()
        {
            OnEnterGameplayState?.Invoke();
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

        public void EnterPreviewState()
        {
            OnEnterPreviewState?.Invoke();
        }
    }
}
