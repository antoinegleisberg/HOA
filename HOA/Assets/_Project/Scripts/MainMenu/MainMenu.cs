using UnityEngine;

namespace antoinegleisberg.HOA.MainMenu
{
    internal class MainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuManager _mainMenuManager;

        public void NewGameButtonAction()
        {
            _mainMenuManager.ShowNewGameMenu();
        }

        public void LoadSaveButtonAction()
        {
            _mainMenuManager.ShowSelectSaveMenu();
        }

        public void QuitGameButtonAction()
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }
    }
}
