using antoinegleisberg.HOA.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using antoinegleisberg.Types;

namespace antoinegleisberg.HOA.UI
{
    public class WorkerInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _recipeName;

        [SerializeField] private Button _nextRecipeButton;
        [SerializeField] private Button _previousRecipeButton;

        private int _selectedRecipeIndex;
        
        private Workplace _workplace;
        private Citizen _citizen;

        public void SetData(Workplace workplace, Citizen citizen)
        {
            if (workplace.TryGetComponent(out ProductionSite productionSite))
            {
                _recipeName.text = productionSite.GetRecipe(citizen).Name;
                _selectedRecipeIndex = productionSite.AvailableRecipes.IndexOf(productionSite.GetRecipe(citizen));
                _nextRecipeButton.gameObject.SetActive(true);
                _previousRecipeButton.gameObject.SetActive(true);
            }
            else
            {
                _recipeName.text = "Worker";
                _nextRecipeButton.gameObject.SetActive(false);
                _previousRecipeButton.gameObject.SetActive(false);
            }
        }

        public void ChangeRecipe(int increment)
        {
            _selectedRecipeIndex += increment;
            ProductionSite productionSite = _workplace.GetComponent<ProductionSite>();
            productionSite.SetRecipe(_citizen, productionSite.AvailableRecipes[_selectedRecipeIndex]);
        }
    }
}
