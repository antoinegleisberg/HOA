using System;
using UnityEngine;

namespace antoinegleisberg.HOA.EventSystems.SceneManagement
{
    public class SceneManagementEvents : MonoBehaviour
    {
        public static SceneManagementEvents Instance { get; private set; }

        public event Action<string> OnStartGame;
        public event Action OnSaveAndQuit;

        private void Awake()
        {
            Instance = this;
        }

        public void StartGame(string saveName)
        {
            OnStartGame?.Invoke(saveName);
        }

        public void SaveAndQuit()
        {
            OnSaveAndQuit?.Invoke();
        }
    }
}
