using antoinegleisberg.HOA.EventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace antoinegleisberg.HOA.UI
{
    [Serializable]
    public enum BuildMenuCategory
    {
        Residential,
        Food,
        Production,
        Special
    }

    public class BuildMenu : MonoBehaviour
    {
        [SerializeField] private Transform _buildMenuItemsContainer;
        [SerializeField] private BuildMenuItem _buildMenuItemPrefab;

        [SerializeField] private Transform _buildMenuCategoriesContainer;
        [SerializeField] private BuildMenuCategoryButton _buildMenuCategoryButtonPrefab;

        [SerializeField] private BuildMenuDetails _buildMenuDetails;

        private Dictionary<BuildMenuItem, BuildMenuCategory> _buildMenuItems;
        private BuildMenuCategory _activeCategory = BuildMenuCategory.Residential;
        private BuildMenuItem _selectedBuilding;

        private void Awake()
        {
            foreach (Transform child in _buildMenuItemsContainer)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in _buildMenuCategoriesContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnEnable()
        {
            if (_buildMenuItems == null)
            {
                CreateBuildMenuItems();
                CreateBuildMenuCategoriesButtons();
            }

            UpdateBuildingsListDisplay();
        }

        public void SelectBuildOnCurrentBuilding()
        {
            UIEvents.Instance.SelectBuildingToBuild(_selectedBuilding.ScriptableBuilding.Name);
        }

        public void SwitchSelectedBuilding(BuildMenuItem item)
        {
            _selectedBuilding = item;
            _buildMenuDetails.UpdateDetails(item.ScriptableBuilding);
        }

        public void SwitchCategory(BuildMenuCategory category)
        {
            _activeCategory = category;
            UpdateBuildingsListDisplay();
        }

        public BuildMenuCategory GetBuildMenuCategory(ScriptableBuilding building)
        {
            if (building.BuildingPrefab.IsHouse)
            {
                return BuildMenuCategory.Residential;
            }
            if (building.BuildingPrefab.IsMainStorage)
            {
                return BuildMenuCategory.Special;
            }
            if (building.BuildingPrefab.IsWorkplace)
            {
                return BuildMenuCategory.Production;
            }
            return BuildMenuCategory.Special;
        }

        private void UpdateBuildingsListDisplay()
        {
            _selectedBuilding = null;
            foreach (BuildMenuItem item in _buildMenuItems.Keys)
            {
                if (_buildMenuItems[item] == _activeCategory)
                {
                    item.gameObject.SetActive(true);
                    if (_selectedBuilding == null)
                    {
                        _selectedBuilding = item;
                    }
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
            SwitchSelectedBuilding(_selectedBuilding);
        }

        private void CreateBuildMenuCategoriesButtons()
        {
            HashSet<BuildMenuCategory> seen = new HashSet<BuildMenuCategory>();
            foreach (ScriptableBuilding building in ScriptableBuildingsDB.GetAllBuildings())
            {
                BuildMenuCategory category = GetBuildMenuCategory(building);
                if (seen.Contains(category)) {
                    continue;
                }
                BuildMenuCategoryButton button = Instantiate(_buildMenuCategoryButtonPrefab, _buildMenuCategoriesContainer);
                button.Init(this, category);
                seen.Add(category);
            }
        }
        
        private void CreateBuildMenuItems()
        {
            _buildMenuItems = new Dictionary<BuildMenuItem, BuildMenuCategory>();
            foreach (ScriptableBuilding building in ScriptableBuildingsDB.GetAllBuildings())
            {
                BuildMenuCategory category = GetBuildMenuCategory(building);
                
                // Missing food category => ToDo later
                BuildMenuItem item = Instantiate(_buildMenuItemPrefab, _buildMenuItemsContainer);
                item.Init(building);
                item.OnClick += () => SwitchSelectedBuilding(item);
                _buildMenuItems[item] = category;
            }
        }
    }
}
