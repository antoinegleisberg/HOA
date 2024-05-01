using UnityEngine;
using UnityEngine.UI;
using TMPro;
using antoinegleisberg.HOA.EventSystem;


namespace antoinegleisberg.HOA.UI
{
    public class BuildMenuItem : MonoBehaviour
    {
        [SerializeField] private Button _onClickButton;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;

        public void Init(ScriptableBuilding building)
        {
            _icon.sprite = building.Sprite;
            _nameText.text = building.Name;
            _onClickButton.onClick.AddListener(() => UIEvents.Instance.SelectBuildingToBuild(building.Name));
        }
    }
}
