using System.Collections;
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

        public void BuildBuilding(ScriptableBuilding scriptableBuilding, Vector2 interpolatedCellPosition)
        {
            Building buildingPrefab = scriptableBuilding.BuildingPrefab;
            
            List<Vector2Int> occupiedTiles = BuildingsPlacer.GetOccupiedTiles(interpolatedCellPosition, scriptableBuilding.Size);

            Vector2 buildingCenter = BuildingsPlacer.GetBuildingCenterWorldCoordinates(interpolatedCellPosition, scriptableBuilding.Size);

            Building instance = Instantiate(buildingPrefab, new Vector3(buildingCenter.x, buildingCenter.y), Quaternion.identity, _buildingsContainer);
            instance.name = scriptableBuilding.Name;

            BuildingsDB.Instance.AddBuilding(instance, occupiedTiles);
        }
    }
}
