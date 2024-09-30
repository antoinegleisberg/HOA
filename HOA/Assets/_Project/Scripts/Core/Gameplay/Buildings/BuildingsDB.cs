using antoinegleisberg.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [RequireComponent(typeof(SaveableEntity))]
    public partial class BuildingsDB : MonoBehaviour, ISaveable
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

        public void RemoveBuilding(Building building)
        {
            if (_buildingTiles.ContainsKey(building))
            {
                foreach (Vector2Int tilePos in _buildingTiles[building])
                {
                    _tileOccupation.Remove(tilePos);
                }
                _buildingTiles.Remove(building);
                _buildings.Remove(building);
            }
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

        /// <summary>
        /// Indicates in which storages the item can be found.
        /// </summary>
        /// <returns></returns>
        public ItemStorageInfo GetLocationOfResource(ScriptableItem item)
        {
            Dictionary<Storage, int> locations = new Dictionary<Storage, int>();
            foreach (Building building in _buildings)
            {
                if (!building.IsStorage)
                {
                    continue;
                }

                Storage storage = building.GetComponent<Storage>();
                IReadOnlyDictionary<ScriptableItem, int> availableItems = storage.AvailableItemsToTake();
                if (availableItems.ContainsKey(item))
                {
                    locations.Add(storage, availableItems[item]);
                }
            }
            return new ItemStorageInfo
            {
                Item = item,
                Availability = locations
            };
        }

        /// <summary>
        /// Indicates which storages have capacity to store the item in question.
        /// </summary>
        /// <returns></returns>
        public ItemStorageInfo GetAvailableMainStorage(ScriptableItem item, int amount)
        {
            Dictionary<Storage, int> locations = new Dictionary<Storage, int>();
            foreach (Building building in _buildings)
            {
                if (!building.IsMainStorage)
                {
                    continue;
                }

                Storage storage = building.GetComponent<Storage>();
                int availableStorage = storage.GetCapacityForItem(item, amount);
                if (availableStorage > 0)
                {
                    locations.Add(storage, availableStorage);
                }
            }
            return new ItemStorageInfo
            {
                Item = item,
                Availability = locations
            };
        }

        public House GetAvailableHouse()
        {
            foreach (Building building in _buildings)
            {
                if (building.IsHouse)
                {
                    if (!building.GetComponent<House>().IsFull)
                    {
                        return building.GetComponent<House>();
                    }
                }
            }
            return null;
        }
        
        public IEnumerable<Building> GetAllBuildings()
        {
            return _buildings;
        }

        public void LoadData(object data)
        {
            BuildingsDBSaveData buildingsDBSaveData = (BuildingsDBSaveData)data;
            for (int i = 0; i < buildingsDBSaveData.Buildings.Length; i++)
            {
                BuildingSaveData buildingSaveData = buildingsDBSaveData.Buildings[i];
                Vector3 position = new Vector3(buildingSaveData.WorldPosition[0], buildingSaveData.WorldPosition[1], buildingSaveData.WorldPosition[2]);
                Building building;
                if (buildingSaveData.IsConstructionSite)
                {
                    building = BuildingsBuilder.Instance.BuildBuildingBuildsite(buildingSaveData.BuildingName, position);
                }
                else
                {
                    building = BuildingsBuilder.Instance.SpawnBuilding(buildingSaveData.BuildingName, position);
                }
                building.GetComponent<GuidHolder>().UniqueId = buildingSaveData.Guid;

                Dictionary<ScriptableItem, int> items = new Dictionary<ScriptableItem, int>();
                foreach (string itemName in buildingSaveData.Items.Keys)
                {
                    ScriptableItem item = ScriptableItemsDB.GetItemByName(itemName);
                    int amount = buildingSaveData.Items[itemName];
                    items.Add(item, amount);
                }
                StartCoroutine(LoadBuildingStorageData(building, items));
            }
        }

        public object GetSaveData()
        {
            BuildingsDBSaveData buildingsDBSaveData = new BuildingsDBSaveData();
            buildingsDBSaveData.Buildings = new BuildingSaveData[_buildings.Count];
            for (int i = 0; i < _buildings.Count; i++)
            {
                buildingsDBSaveData.Buildings[i] = new BuildingSaveData(_buildings[i]);
            }
            return buildingsDBSaveData;
        }

        private IEnumerator LoadBuildingStorageData(Building building, Dictionary<ScriptableItem, int> items)
        {
            if (building.IsStorage)
            {
                yield return new WaitUntil(() => building.GetComponent<Storage>().InitializedStorage);
                building.GetComponent<Storage>().AddItems(items);
            }
            else if (items.Count > 0)
            {
                Debug.LogError("Unable to find building storage");
            }
        }
    }
}