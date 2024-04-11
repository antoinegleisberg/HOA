using System.Collections.Generic;
using UnityEngine;
using antoinegleisberg.HOA.Saving;

namespace antoinegleisberg.HOA.MainMenu
{
    internal class SelectSaveMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuManager _mainMenuManager;

        [SerializeField] private RectTransform _loadSaveButtonsContainer;
        [SerializeField] private LoadSaveButton _loadSaveButtonPrefab;

        private List<LoadSaveButton> _loadSaveButtons;

        private void OnEnable()
        {
            RefreshSaveButtons();
        }

        private void OnDisable()
        {
            DestroyLoadSaveButtons();
        }

        private void OnLoadSaveButtonClicked(string saveName)
        {
            _mainMenuManager.StartGame(saveName);
        }

        private void RefreshSaveButtons()
        {
            DestroyLoadSaveButtons();
            CreateLoadSaveButtons();
        }

        private void DestroyLoadSaveButtons()
        {
            foreach (Transform child in _loadSaveButtonsContainer)
            {
                child.GetComponent<LoadSaveButton>().OnLoadSaveButtonClicked -= OnLoadSaveButtonClicked;
                Destroy(child.gameObject);
            }
            _loadSaveButtons = null;
        }

        private void CreateLoadSaveButtons()
        {
            List<string> saveNames = SaveManager.GetSaveNames();
            _loadSaveButtons = new List<LoadSaveButton>();

            foreach (string saveName in saveNames)
            {
                LoadSaveButton loadSaveButton = Instantiate(_loadSaveButtonPrefab, _loadSaveButtonsContainer);
                SaveInfo saveInfo = SaveManager.GetSave(saveName);
                loadSaveButton.UpdateButton(saveInfo);
                loadSaveButton.OnLoadSaveButtonClicked += OnLoadSaveButtonClicked;
                _loadSaveButtons.Add(loadSaveButton);
            }
        }
    }
}
