using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace antoinegleisberg.HOA.UI
{
    public class BuildMenuCostLine : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _icon;

        public void SetData(ScriptableItem item, int amount)
        {
            _text.text = $"{item.Name}: {amount}";
            _icon.sprite = item.Icon;
        }
    }
}
