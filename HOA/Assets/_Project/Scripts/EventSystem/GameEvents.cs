using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Instance { get; private set; }

        public event Action OnEnterGameplayState;
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
