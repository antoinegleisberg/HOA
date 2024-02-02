using antoinegleisberg.SaveSystem;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(SaveableEntity))]
    public class BuildingsDB : MonoBehaviour, ISaveable
    {
        public static BuildingsDB Instance { get; private set; }

        private Dictionary<Vector2Int, Building> _tileOccupation;
        private Dictionary<Building, List<Vector2Int>> _buildingTiles;
        private List<Building> _buildings;

        private void Awake()
        {
            Instance = this;

            _tileOccupation = new Dictionary<Vector2Int, Building>();
            _buildingTiles = new Dictionary<Building, List<Vector2Int>>();
            _buildings = new List<Building>();
        }

        public bool TileIsOccupied(Vector2Int gridPos)
        {
            return _tileOccupation.ContainsKey(gridPos);
        }

        public void AddBuilding(Building building, List<Vector2Int> occupiedTiles)
        {
            foreach (Vector2Int tilePos in occupiedTiles)
            {
                _tileOccupation.Add(tilePos, building);
            }
            _buildingTiles.Add(building, occupiedTiles);
            _buildings.Add(building);
        }

        public Building GetBuildingWithComponentOfType<T>()
        {
            foreach (Building building in _buildings)
            {
                if (building.GetComponent<T>() != null)
                {
                    return building;
                }
            }
            return null;
        }

        public List<Workplace> GetAvailableWorkplaces()
        {
            List<Workplace> workplaces = new List<Workplace>();
            foreach (Building building in _buildings)
            {
                Workplace workplace = building.GetComponent<Workplace>();
                if (workplace != null && workplace.RemainingWorkersSpace() > 0)
                {
                    workplaces.Add(workplace);
                }
            }
            return workplaces;
        }

        public MainStorage GetAvailableMainStorage(ScriptableItem itemToAdd)
        {
            foreach (Building building in _buildings)
            {
                if (building.IsMainStorage)
                {
                    MainStorage storage = building.GetComponent<MainStorage>();
                    if (storage.Inventory.CanAddItem(itemToAdd))
                    {
                        return storage;
                    }
                }
            }
            return null;
        }

        public void LoadData(object data)
        {
            BuildingsDBSaveData buildingsDBSaveData = (BuildingsDBSaveData)data;
            for (int i = 0; i < buildingsDBSaveData.Buildings.Length; i++)
            {
                BuildingSaveData buildingSaveData = buildingsDBSaveData.Buildings[i];
                Vector3 position = new Vector3(buildingSaveData.WorldPosition[0], buildingSaveData.WorldPosition[1], buildingSaveData.WorldPosition[2]);
                Building building = BuildingsBuilder.Instance.BuildBuilding(buildingSaveData.BuildingName, position);
                building.GetComponent<GuidHolder>().UniqueId = buildingSaveData.Guid;
            }
        }

        public object GetSaveData()
        {
            BuildingsDBSaveData buildingsDBSaveData = new BuildingsDBSaveData();
            buildingsDBSaveData.Buildings = new BuildingSaveData[_buildings.Count];
            for (int i = 0; i < _buildings.Count; i++)
            {
                float[] position = new float[3] { _buildings[i].transform.position.x, _buildings[i].transform.position.y, _buildings[i].transform.position.z };
                BuildingSaveData buildingSaveData = new BuildingSaveData()
                {
                    BuildingName = _buildings[i].ScriptableBuilding.Name,
                    WorldPosition = position,
                    Guid = _buildings[i].GetComponent<GuidHolder>().UniqueId
                };
                buildingsDBSaveData.Buildings[_buildings.IndexOf(_buildings[i])] = buildingSaveData;
            }
            return buildingsDBSaveData;
        }

        [System.Serializable]
        private struct BuildingSaveData
        {
            public string BuildingName;
            public float[] WorldPosition;
            public string Guid;
        }

        [System.Serializable]
        private struct BuildingsDBSaveData
        {
            public BuildingSaveData[] Buildings;
        }
    }
}