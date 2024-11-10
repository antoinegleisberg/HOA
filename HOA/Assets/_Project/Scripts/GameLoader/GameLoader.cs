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
        private static readonly string GAMEPLAY_SCENE_NAME = "Gameplay";
        private static readonly string MAIN_MENU_SCENE_NAME = "MainMenu";

        
        private void Start()
        {
            SceneManagementEvents.Instance.OnStartGame += LoadGame;
            SceneManagementEvents.Instance.OnSaveAndQuit += SaveAndQuitGame;
            
            if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(GAMEPLAY_SCENE_NAME).isLoaded)
            {
                // In the editor, we may open the gameplay scene without going through the main menu first
                CurrentSaveName = _defaultSaveName;
                string gameDataPath = SaveManager.GetRelativeGameDataPath(CurrentSaveName);
                SaveSystem.LoadSave(gameDataPath);
                return;
            }

            SceneManager.Instance.LoadScene(MAIN_MENU_SCENE_NAME);
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
            SceneManager.Instance.UnloadScenes(MAIN_MENU_SCENE_NAME);
            SceneManager.Instance.LoadScene(GAMEPLAY_SCENE_NAME, () => SaveSystem.LoadSave(gameDataPath));
        }

        private void SaveAndQuitGame()
        {
            string gameDataPath = SaveManager.GetRelativeGameDataPath(CurrentSaveName);
            SaveSystem.SaveGame(gameDataPath);
            SceneManager.Instance.UnloadScenes(GAMEPLAY_SCENE_NAME);
            SceneManager.Instance.LoadScene(MAIN_MENU_SCENE_NAME);
        }
    }
}
