using UnityEngine;
using TMPro;
using System.Collections;
using antoinegleisberg.HOA.Saving;

namespace antoinegleisberg.HOA.MainMenu
{
    internal class NewGameMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuManager _mainMenuManager;

        [SerializeField] private PresetSelector _presetSelector;

        [SerializeField] private TMP_InputField _saveNameInputField;
        [SerializeField] private TextMeshProUGUI _errorMessageText;

        [SerializeField] private float _errorMessageDuration = 5f;

        private Coroutine _showErrorCoroutine;

        private void OnEnable()
        {
            _saveNameInputField.text = "";
            _errorMessageText.text = "";
        }

        public void CreateGameButtonAction()
        {
            string saveName = _saveNameInputField.text;

            SaveNameValidation validation = SaveManager.IsValidNewSaveName(saveName);

            if (_showErrorCoroutine != null)
            {
                StopCoroutine(_showErrorCoroutine);
            }

            if (validation.Success)
            {
                _mainMenuManager.CreateNewGame(saveName, _presetSelector.PresetName);
            }
            else
            {
                _showErrorCoroutine = StartCoroutine(ShowErrorMessage(validation.ErrorMessage));
            }
        }

        private IEnumerator ShowErrorMessage(string message)
        {
            _errorMessageText.text = message;

            yield return new WaitForSeconds(_errorMessageDuration);

            _errorMessageText.text = "";
        }
    }
}
