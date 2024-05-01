using UnityEngine;
using antoinegleisberg.Types;
using TMPro;
using UnityEngine.UI;


namespace antoinegleisberg.HOA.UI
{
    public class BuildMenuDetails : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Image _icon;

        [SerializeField] private BuildMenuCostLine _costLinePrefab;
        [SerializeField] private Transform _costLinesContainer;

        public void UpdateDetails(ScriptableBuilding building)
        {
            _nameText.text = building.Name;
            _descriptionText.text = building.Description;
            _icon.sprite = building.Sprite;
            UpdateCostLines(building);
        }

        private void UpdateCostLines(ScriptableBuilding building)
        {
            foreach (Transform child in _costLinesContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (Pair<ScriptableItem, int> pair in building.BuildingMaterials)
            {
                BuildMenuCostLine costLine = Instantiate(_costLinePrefab, _costLinesContainer);
                costLine.SetData(pair.First, pair.Second);
            }
        }
    }
}
