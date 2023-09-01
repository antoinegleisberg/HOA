using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }

        private Dictionary<Vector2Int, Building> _tileOccupation;
        private Dictionary<Building, List<Vector2Int>> _buildingTiles;
        private List<Building> _buildings;

        [field:SerializeField] public Grid Grid { get; private set; }

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

        public Storage GetMainStorage()
        {
            foreach (Building building in _buildings)
            {
                if (building.GetComponent<Storage>() != null && building.GetComponent<Workplace>() != null && building.GetComponent<ProductionSite>() == null)
                {
                    return building.GetComponent<Storage>();
                }
            }
            return null;
        }
    }
}
