using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager instance { get; private set; }

    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _tilesParent;
    [SerializeField] private Transform _buildingsParent;
    private Dictionary<Vector2Int, Tile> _tiles;
    private Dictionary<Vector2Int, BaseBuilding> _buildings;
    private List<BaseBuilding> _buildingsList;


    private void Awake()
    {
        instance = this;
        _tiles = new Dictionary<Vector2Int, Tile>();
        _buildings = new Dictionary<Vector2Int, BaseBuilding>();
        _buildingsList = new List<BaseBuilding>();
    }

    public void GenerateGrid()
    {
        float tileWidth = _tilePrefab.transform.localScale.x;
        float tileHeight = _tilePrefab.transform.localScale.y;
        for (int x=0; x<_width; x++)
        {
            for (int y=0; y< _height; y++)
            {
                Tile spawnedTile = Instantiate(_tilePrefab, new Vector3(x+tileWidth/2, y+tileHeight/2, 0), Quaternion.identity, _tilesParent);
                spawnedTile.name = $"Tile {x} {y}";
                Vector2Int tilePosition = new Vector2Int(x, y);
                spawnedTile.Init(tilePosition);
                _tiles[tilePosition] = spawnedTile;
            }
        }
    }

    public Tile GetTile(Vector2Int pos)
    {
        pos = new Vector2Int(pos.x, pos.y);
        if (instance._tiles.TryGetValue(pos, out Tile tile))
        {
            return tile;
        }
        return null;
    }

    public Tile GetTile(float x, float y) { return GetTile(new Vector2Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y))); }

    public Tile GetTile(Vector3 pos) { return GetTile(pos.x, pos.y); }

    public bool IsOccupied(Vector2Int pos) { return _buildings.ContainsKey(pos); }

    public void BuildBuilding(string buildingName, Vector3 clickPosition)
    {
        ScriptableBuilding scriptableBuilding = BuildingUtilities.GetBuildingByName(buildingName);
        BaseBuilding buildingPrefab = scriptableBuilding.BuildingPrefab;
        List<Vector2Int> tilePositions = BuildingUtilities.GetOccupiedPositions(clickPosition, buildingName);
        Vector2 position = new Vector2();
        Vector2Int size = scriptableBuilding.size;
        position.x = size.x % 2 == 0 ? Mathf.Floor(clickPosition.x + 0.5f) : Mathf.Floor(clickPosition.x) + 0.5f;
        position.y = size.y % 2 == 0 ? Mathf.Floor(clickPosition.y + 0.5f) : Mathf.Floor(clickPosition.y) + 0.5f;
        BaseBuilding building = Instantiate(buildingPrefab, position, Camera.main.transform.rotation, _buildingsParent);
        building.name = $"{buildingName} {position.x} {position.y}";
        foreach (Vector2Int pos in tilePositions) { _buildings[pos] = building; }
        _buildingsList.Add(building);
        building.Init();
    }

    public List<BaseBuilding> GetBuildingsList() => _buildingsList;

    public BaseBuilding GetBuildingAt(Vector2Int pos) { return IsOccupied(pos) ? _buildings[pos] : null; }
}
