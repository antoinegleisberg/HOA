using antoinegleisberg.Types;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    public class BuildingsBuilder : MonoBehaviour
    {
        public static BuildingsBuilder Instance { get; private set; }

        [SerializeField] private Transform _buildingsContainer;

        [SerializeField] private Building _constructionSitePrefab;

        private void Awake()
        {
            Instance = this;
        }

        public Building SpawnBuilding(string buildingName, Vector3 worldPosition)
        {
            ScriptableBuilding scriptableBuilding = ScriptableBuildingsDB.GetBuildingByName(buildingName);
            if (scriptableBuilding != null)
            {
                return SpawnBuilding(scriptableBuilding, worldPosition);
            }
            else
            {
                throw new Exception($"No building with name {buildingName} found in the database.");
            }
        }

        public Building BuildBuildingBuildsite(string buildingName, Vector3 worldPosition)
        {
            ScriptableBuilding scriptableBuilding = ScriptableBuildingsDB.GetBuildingByName(buildingName);
            if (scriptableBuilding != null)
            {
                return BuildBuildingBuildsite(scriptableBuilding, worldPosition);
            }
            else
            {
                throw new Exception($"No building with name {buildingName} found in the database.");
            }
        }

        public Building BuildBuildingBuildsite(ScriptableBuilding scriptableBuilding, Vector3 worldPosition)
        {
            Building building = InstantiateBuilding(_constructionSitePrefab, scriptableBuilding.Size, worldPosition, scriptableBuilding.Name + " Construction Site");
            building.Initialize(scriptableBuilding);
            return building;
        }

        public Building BuildBuilding(ConstructionSite buildsite)
        {
            Vector3 position = buildsite.transform.position;
            ScriptableBuilding scriptableBuilding = buildsite.GetComponent<Building>().ScriptableBuilding;
            
            DestroyBuilding(buildsite.GetComponent<Building>());

            return SpawnBuilding(scriptableBuilding, position);
        }

        public Building SpawnBuilding(ScriptableBuilding scriptableBuilding, Vector3 worldPosition)
        {
            return InstantiateBuilding(scriptableBuilding.BuildingPrefab, scriptableBuilding.Size, worldPosition, scriptableBuilding.Name);
        }

        public void DestroyBuilding(Building building)
        {
            BuildingsDB.Instance.RemoveBuilding(building);
            Destroy(building.gameObject);
        }

        private Building InstantiateBuilding(Building prefab, Vector2Int buildingSize, Vector3 worldPosition, string name)
        {
            Vector2 interpolatedCellPosition = GridManager.Instance.WorldToInterpolatedCellPosition(worldPosition);
            List<Vector2Int> occupiedTiles = BuildingsPlacer.GetOccupiedTiles(interpolatedCellPosition, buildingSize);
            Pair<Pair<int, int>, Pair<int, int>> occupationRange = BuildingsPlacer.GetOccupationRange(interpolatedCellPosition, buildingSize);
            Vector2 buildingCenter = BuildingsPlacer.GetBuildingCenterWorldCoordinates(interpolatedCellPosition, buildingSize);

            Building instance = Instantiate(prefab, new Vector3(buildingCenter.x, buildingCenter.y), Quaternion.identity, _buildingsContainer);
            instance.name = name;

            BuildingsDB.Instance.AddBuilding(instance, occupiedTiles);
            PathfindingGraph.Instance.RemoveNodeRange(occupationRange);

            return instance;
        }
    }
}
