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
            
            if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(_gameplayScene).isLoaded)
            {
                // In the editor, we may open the gameplay scene without going through the main menu first
                CurrentSaveName = _defaultSaveName;
                string gameDataPath = SaveManager.GetRelativeGameDataPath(CurrentSaveName);
                SaveSystem.LoadSave(gameDataPath);
                return;
            }

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
