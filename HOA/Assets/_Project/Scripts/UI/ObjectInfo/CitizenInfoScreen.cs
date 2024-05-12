using System.Collections;
using TMPro;
using UnityEngine;
using antoinegleisberg.HOA.Core;

namespace antoinegleisberg.HOA.UI
{
    public class CitizenInfoScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _hungerText;
        [SerializeField] private TextMeshProUGUI _thirstText;

        [SerializeField] private TextMeshProUGUI _workplaceText;

        public void SetData(Citizen citizen)
        {
            _hungerText.text = $"Is {(citizen.IsHungry ? "" : "not")} hungry";
            _thirstText.text = $"Is {(citizen.IsThirsty ? "" : "not")} thirsty";

            _workplaceText.text = $"Works at {citizen.Workplace?.GetComponent<Building>().ScriptableBuilding.Name ?? "nowhere"}";
        }
    }
}
