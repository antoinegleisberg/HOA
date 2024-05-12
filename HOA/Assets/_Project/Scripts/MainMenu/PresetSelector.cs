using antoinegleisberg.HOA.Saving;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace antoinegleisberg.HOA.MainMenu
{
    public class PresetSelector : MonoBehaviour
    {
        [SerializeField] private Button _leftChangePresetButton;
        [SerializeField] private Button _rightChangePresetButton;

        [SerializeField] private TextMeshProUGUI _presetNameText;
        
        private int _currentPresetIndex;

        public string PresetName => _presetNameText.text;
        
        private void Awake()
        {
            _leftChangePresetButton.onClick.AddListener(() => ChangePreset(-1));
            _rightChangePresetButton.onClick.AddListener(() => ChangePreset(1));
            _currentPresetIndex = 0;
        }

        private void OnEnable()
        {
            UpdatePresetUI();
        }

        private void ChangePreset(int change)
        {
            _currentPresetIndex += change;
            UpdatePresetUI();
        }

        private void UpdatePresetUI()
        {
            List<PresetInfo> presets = SaveManager.GetPresets();
            _currentPresetIndex = _currentPresetIndex % presets.Count;
            _presetNameText.text = presets[_currentPresetIndex].PresetName;
        }
    }
}