using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using antoinegleisberg.Math.ProceduralGeneration;
namespace antoinegleisberg.HOA.Core
{
    /// <summary>
    /// Generates the map
    /// Contains the tile generation rules
    /// </summary>
    public class MapGenerator : MonoBehaviour
    {
        private LayeredSimplexNoise _noiseGenerator;

        [SerializeField] private TileData _groundTile;
        [SerializeField] private TileData _waterTile;

        [SerializeField] private TileType _waterTileType;

        [SerializeField] private int _noiseSeed;
        [SerializeField] private float _noiseFrequency;
        [SerializeField] private float _waterThreshold;

        private void Awake()
        {
            _noiseGenerator = new LayeredSimplexNoise(_noiseSeed);
        }
        
        private void OnValidate()
        {
            _noiseGenerator = new LayeredSimplexNoise(_noiseSeed);
        }

        public void GenerateAt(Vector3Int position)
        {
            if (MapManager.Instance.IsTileGenerated(position))
            {
                return;
            }
            
            float noiseValue = _noiseGenerator.Generate(new List<float>() { position.x, position.y }, initialFrequency: _noiseFrequency);
            TileData tileData = noiseValue < _waterThreshold ? _waterTile : _groundTile;

            MapManager.Instance.SetTileAt(position, tileData);

            // For water tiles, always generate adjacent tiles too
            if (tileData.TileType == _waterTileType)
            {
                GenerateAt(position + Vector3Int.right);
                GenerateAt(position + Vector3Int.left);
                GenerateAt(position + Vector3Int.up);
                GenerateAt(position + Vector3Int.down);
                GenerateAt(position + Vector3Int.right + Vector3Int.up);
                GenerateAt(position + Vector3Int.right + Vector3Int.down);
                GenerateAt(position + Vector3Int.left + Vector3Int.up);
                GenerateAt(position + Vector3Int.left + Vector3Int.down);

                Remove1WideWaterTilesAround(position);
            }
        }

        private void Remove1WideWaterTilesAround(Vector3Int position)
        {
            if (!MapManager.Instance.IsTileGenerated(position))
            {
                return;
            }
            if (MapManager.Instance.GetTileAt(position) != _waterTile)
            {
                return;
            }

            if ((MapManager.Instance.GetTileAt(position + Vector3Int.right) != _waterTile) && (MapManager.Instance.GetTileAt(position + Vector3Int.left) != _waterTile))
            {
                MapManager.Instance.SetTileAt(position, _groundTile);
                Remove1WideWaterTilesAround(position + Vector3Int.up);
                Remove1WideWaterTilesAround(position + Vector3Int.down);
            }
            if ((MapManager.Instance.GetTileAt(position + Vector3Int.up) != _waterTile) && (MapManager.Instance.GetTileAt(position + Vector3Int.down) != _waterTile))
            {
                MapManager.Instance.SetTileAt(position, _groundTile);
                Remove1WideWaterTilesAround(position + Vector3Int.right);
                Remove1WideWaterTilesAround(position + Vector3Int.left);
            }
            if ((MapManager.Instance.GetTileAt(position + Vector3Int.right) != _waterTile) && (MapManager.Instance.GetTileAt(position + Vector3Int.up) != _waterTile) && (MapManager.Instance.GetTileAt(position + Vector3Int.down + Vector3Int.left) != _waterTile))
            {
                MapManager.Instance.SetTileAt(position, _groundTile);
                Remove1WideWaterTilesAround(position + Vector3Int.down);
                Remove1WideWaterTilesAround(position + Vector3Int.left);
            }
            if ((MapManager.Instance.GetTileAt(position + Vector3Int.right) != _waterTile) && (MapManager.Instance.GetTileAt(position + Vector3Int.down) != _waterTile) && (MapManager.Instance.GetTileAt(position + Vector3Int.up + Vector3Int.left) != _waterTile))
            {
                MapManager.Instance.SetTileAt(position, _groundTile);
                Remove1WideWaterTilesAround(position + Vector3Int.up);
                Remove1WideWaterTilesAround(position + Vector3Int.left);
            }
            if ((MapManager.Instance.GetTileAt(position + Vector3Int.left) != _waterTile) && (MapManager.Instance.GetTileAt(position + Vector3Int.up) != _waterTile) && (MapManager.Instance.GetTileAt(position + Vector3Int.down + Vector3Int.right) != _waterTile))
            {
                MapManager.Instance.SetTileAt(position, _groundTile);
                Remove1WideWaterTilesAround(position + Vector3Int.down);
                Remove1WideWaterTilesAround(position + Vector3Int.right);
            }
            if ((MapManager.Instance.GetTileAt(position + Vector3Int.left) != _waterTile) && (MapManager.Instance.GetTileAt(position + Vector3Int.down) != _waterTile) && (MapManager.Instance.GetTileAt(position + Vector3Int.up + Vector3Int.right) != _waterTile))
            {
                MapManager.Instance.SetTileAt(position, _groundTile);
                Remove1WideWaterTilesAround(position + Vector3Int.up);
                Remove1WideWaterTilesAround(position + Vector3Int.right);
            }
        }
    }
}
