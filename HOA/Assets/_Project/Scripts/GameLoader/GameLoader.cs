using UnityEngine;
using antoinegleisberg.HOA.Saving;
using antoinegleisberg.HOA.EventSystems.SceneManagement;
using antoinegleisberg.SceneManagement;
using antoinegleisberg.Saving;

namespace antoinegleisberg.HOA.GameLoader
{
    public class GameLoader : MonoBehaviour
    {
        [field: SerializeField] public string CurrentSaveName { get; private set; }

        [SerializeField] private string _defaultSaveName;

        [SerializeField] private SceneField _mainMenuScene;
        [SerializeField] private SceneField _gameplayScene;
        
        private void Start()
        {
            SceneManagementEvents.Instance.OnStartGame += LoadGame;
            SceneManagementEvents.Instance.OnSaveAndQuit += SaveAndQuitGame;

#if UNITY_EDITOR

            bool isGameplaySceneLoaded = UnityEngine.SceneManagement.SceneManager.GetSceneByName(_gameplayScene).isLoaded;
            bool isMainMenuSceneLoaded = UnityEngine.SceneManagement.SceneManager.GetSceneByName(_mainMenuScene).isLoaded;
            if (isGameplaySceneLoaded && isMainMenuSceneLoaded)
            {
                // If both are loaded for debugging purposes, unload gameplay scene
                // So we go to the main menu
                SceneManager.Instance.UnloadScenes(_gameplayScene);
                return;
            }
            if (isGameplaySceneLoaded)
            {
                // If only the gameplay scene is loaded, we skip the main menu
                // And immediately go to gameplay
                CurrentSaveName = _defaultSaveName;
                string gameDataPath = SaveManager.GetRelativeGameDataPath(CurrentSaveName);
                SaveSystem.LoadSave(gameDataPath);
                return;
            }
#endif

            SceneManager.Instance.LoadScene(_mainMenuScene);
        }

        private void OnDestroy()
        {
            SceneManagementEvents.Instance.OnStartGame -= LoadGame;
            SceneManagementEvents.Instance.OnSaveAndQuit -= SaveAndQuitGame;
        }

        private void LoadGame(string saveName)
        {
            CurrentSaveName = saveName;
            string gameDataPath = SaveManager.GetRelativeGameDataPath(CurrentSaveName);
            SceneManager.Instance.UnloadScenes(_mainMenuScene);
            SceneManager.Instance.LoadScene(_gameplayScene, () => SaveSystem.LoadSave(gameDataPath));
        }

        private void SaveAndQuitGame()
        {
            string gameDataPath = SaveManager.GetRelativeGameDataPath(CurrentSaveName);
            SaveSystem.SaveGame(gameDataPath);
            SceneManager.Instance.UnloadScenes(_gameplayScene);
            SceneManager.Instance.LoadScene(_mainMenuScene);
        }
    }
}
