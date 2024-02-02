using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class BuildingsBuilder : MonoBehaviour
    {
        public static BuildingsBuilder Instance { get; private set; }

        [SerializeField] private Transform _buildingsContainer;

        private void Awake()
        {
            Instance = this;
        }

        public Building BuildBuilding(string name, Vector3 worldPosition)
        {
            ScriptableBuilding scriptableBuilding = ScriptableBuildingsDB.GetBuildingByName(name);
            if (scriptableBuilding != null)
            {
                return BuildBuilding(scriptableBuilding, worldPosition);
            }
            else
            {
                Debug.LogError($"No building with name {name} found in the database.");
                return null;
            }
        }

        public Building BuildBuilding(ScriptableBuilding scriptableBuilding, Vector3 worldPosition)
        {
            Vector2 interpolatedCellPosition = GridManager.Instance.WorldToInterpolatedCellPosition(worldPosition);

            Building buildingPrefab = scriptableBuilding.BuildingPrefab;
            
            List<Vector2Int> occupiedTiles = BuildingsPlacer.GetOccupiedTiles(interpolatedCellPosition, scriptableBuilding.Size);

            Vector2 buildingCenter = BuildingsPlacer.GetBuildingCenterWorldCoordinates(interpolatedCellPosition, scriptableBuilding.Size);

            Building instance = Instantiate(buildingPrefab, new Vector3(buildingCenter.x, buildingCenter.y), Quaternion.identity, _buildingsContainer);
            instance.name = scriptableBuilding.Name;

            BuildingsDB.Instance.AddBuilding(instance, occupiedTiles);

            return instance;
        }
    }
}
