using antoinegleisberg.HOA.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace antoinegleisberg.HOA.UI
{
    public class StorageElement : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private TextMeshProUGUI _quantityText;
        
        public void SetData(ScriptableItem item, int amount)
        {
            _icon.sprite = item.Icon;
            _itemNameText.text = item.Name;
            _quantityText.text = $"{amount}";
        }
    }
}
