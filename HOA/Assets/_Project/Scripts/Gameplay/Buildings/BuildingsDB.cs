using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class BuildingsDB : MonoBehaviour
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
    }
}
