using System;
using TMPro;
using UnityEngine;
using antoinegleisberg.HOA.Saving;

namespace antoinegleisberg.HOA.MainMenu
{
    internal class LoadSaveButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _saveNameText;

        public event Action<string> OnLoadSaveButtonClicked;

        public void UpdateButton(SaveInfo saveInfo)
        {
            _saveNameText.text = saveInfo.SaveName;
        }

        public void LoadSaveButtonAction()
        {
            OnLoadSaveButtonClicked?.Invoke(_saveNameText.text);
        }
    }
}
