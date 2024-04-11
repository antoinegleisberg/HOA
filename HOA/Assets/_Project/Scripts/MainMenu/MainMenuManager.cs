using UnityEngine;
using antoinegleisberg.HOA.Saving;
using antoinegleisberg.HOA.EventSystems.SceneManagement;

namespace antoinegleisberg.HOA.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private NewGameMenu _newGameMenu;
        [SerializeField] private SelectSaveMenu _selectSaveMenu;

        private void Start()
        {
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            _mainMenu.gameObject.SetActive(true);
            _newGameMenu.gameObject.SetActive(false);
            _selectSaveMenu.gameObject.SetActive(false);
        }

        public void ShowNewGameMenu()
        {
            _mainMenu.gameObject.SetActive(false);
            _newGameMenu.gameObject.SetActive(true);
            _selectSaveMenu.gameObject.SetActive(false);
        }
        
        public void ShowSelectSaveMenu()
        {
            _mainMenu.gameObject.SetActive(false);
            _newGameMenu.gameObject.SetActive(false);
            _selectSaveMenu.gameObject.SetActive(true);
        }

        public void StartGame(string saveName)
        {
            Debug.Log("Start game with save name: " + saveName);
            SaveInfo saveInfo = SaveManager.GetSave(saveName);
            SceneManagementEvents.Instance.StartGame(saveInfo.SaveName);
        }

        public void CreateNewGame(string saveName, string presetName)
        {
            SaveInfo saveInfo = SaveManager.CreateNewSave(saveName, presetName);
            StartGame(saveInfo.SaveName);
        }
    }
}
