using UnityEngine;
using UnityEngine.UI;
using TMPro;
using antoinegleisberg.HOA.EventSystem;
using System;
using antoinegleisberg.HOA.Core;


namespace antoinegleisberg.HOA.UI
{
    public class BuildMenuItem : MonoBehaviour
    {
        [SerializeField] private Button _onClickButton;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;

        public ScriptableBuilding ScriptableBuilding { get; private set; }

        public event Action OnClick;

        public void Init(ScriptableBuilding building)
        {
            _icon.sprite = building.Sprite;
            _nameText.text = building.Name;
            ScriptableBuilding = building;
            _onClickButton.onClick.AddListener(() => OnClick?.Invoke());
        }
    }
}
