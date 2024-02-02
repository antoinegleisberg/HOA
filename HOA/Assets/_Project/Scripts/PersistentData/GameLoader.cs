using UnityEngine;
using antoinegleisberg.HOA.MainMenu;
using antoinegleisberg.HOA.Saving;
using antoinegleisberg.SceneManagement;

namespace antoinegleisberg.HOA
{
    public class GameLoader : MonoBehaviour
    {
        public static GameLoader Instance { get; private set; }

        [field: SerializeField] public string CurrentSaveName { get; private set; }

        [SerializeField] private MainMenuManager _mainMenuManager;

        private static readonly string GAMEPLAY_SCENE_NAME = "Gameplay";
        private static readonly string MAIN_MENU_SCENE_NAME = "MainMenu";

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SceneManager.Instance.LoadScene(MAIN_MENU_SCENE_NAME, () => AssignMainMenuManager());
        }

        public void LoadGame(string saveName)
        {
            CurrentSaveName = saveName;
            string gameDataPath = SaveManager.GetRelativeGameDataPath(saveName);
            SceneManager.Instance.UnloadScenes(MAIN_MENU_SCENE_NAME);
            SceneManager.Instance.LoadScene(GAMEPLAY_SCENE_NAME, () => SaveSystem.SaveSystem.LoadSave(gameDataPath));
        }

        public void SaveGame()
        {
            string gameDataPath = SaveManager.GetRelativeGameDataPath(CurrentSaveName);
            SaveSystem.SaveSystem.SaveGame(gameDataPath);
        }

        private void AssignMainMenuManager()
        {
            _mainMenuManager = FindObjectOfType<MainMenuManager>();
            _mainMenuManager.OnStartGame += LoadGame;
        }   
    }
}
