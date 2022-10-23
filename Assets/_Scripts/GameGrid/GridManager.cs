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
    private Dictionary<Vector2, Tile> _tiles;
    private Dictionary<Vector2, BaseBuilding> _buildings;


    private void Awake()
    {
        instance = this;
        _tiles = new Dictionary<Vector2, Tile>();
        _buildings = new Dictionary<Vector2, BaseBuilding>();
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
                Vector2 tilePosition = new Vector2(x, y);
                spawnedTile.Init(tilePosition);
                _tiles[tilePosition] = spawnedTile;
            }
        }
    }

    public Tile GetTile(Vector2 pos)
    {
        pos = new Vector2(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
        if (instance._tiles.TryGetValue(pos, out Tile tile))
        {
            return tile;
        }
        return null;
    }

    public Tile GetTile(float x, float y) { return GetTile(new Vector2(x, y)); }

    public bool IsOccupied(Vector2 pos) { return _buildings.ContainsKey(pos); }

    public BaseBuilding GetBuildingAt(Vector2 pos) { return IsOccupied(pos) ? _buildings[pos] : null; }
}
