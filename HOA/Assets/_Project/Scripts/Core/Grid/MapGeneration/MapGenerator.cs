using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using antoinegleisberg.Math.ProceduralGeneration;
using System.Reflection;
using System;

using Random = UnityEngine.Random;

namespace antoinegleisberg.HOA.Core
{
    public class MapGenerator : MonoBehaviour
    {
        // Split this class into multiple:
        // The MapGenerator that is in charge of generating the initial map, contains the generation rules
        // The MapManager, that handles changes (eg. paths, terraforming, etc), and that saves the tile data
        // and a list of generated tiles
        // The MapDisplayManager, that is in charge of displaying the information of the MapManager

        public static MapGenerator Instance { get; private set; }

        [SerializeField] private int _generationRange;

        [SerializeField] private Tilemap _tilemap;

        private LayeredSimplexNoise _noiseGenerator;

        [SerializeField] private TileData _groundTile;
        [SerializeField] private TileData _waterTile;

        [SerializeField] private TileType _waterTileType;
        
        private Dictionary<Vector3Int, TileData> _generatedTiles;

        [SerializeField] private int _noiseSeed;
        [SerializeField] private float _noiseFrequency;
        [SerializeField] private float _waterThreshold;

        private void Awake()
        {
            Instance = this;
            _noiseGenerator = new LayeredSimplexNoise(_noiseSeed);
            _generatedTiles = new Dictionary<Vector3Int, TileData>();
            Generate();
        }

        public TileData TileAt(Vector3Int position)
        {
            if (!_generatedTiles.ContainsKey(position))
            {
                GenerateAt(position);
            }
            return _generatedTiles[position];
        }

        private void GenerateAt(Vector3Int position)
        {
            if (_generatedTiles.ContainsKey(position))
            {
                // Tile already exists
                return;
            }

            // Generate the tile data
            float noiseValue = _noiseGenerator.Generate(new List<float>() { position.x, position.y }, initialFrequency: _noiseFrequency);
            TileData tileData = noiseValue < _waterThreshold ? _waterTile : _groundTile;

            SetTileAt(position, tileData);

            // For water tiles, always generate adjacent tiles too
            if (tileData.TileType == _waterTileType)
            {
                GenerateAt(position + Vector3Int.right);
                GenerateAt(position + Vector3Int.left);
                GenerateAt(position + Vector3Int.up);
                GenerateAt(position + Vector3Int.down);

                Remove1WideWaterTilesAround(position);

                MapDisplayManager.Instance.UpdateWaterTilesDisplay(position);
            }
        }

        private void SetTileAt(Vector3Int position, TileData tileData)
        {
            _generatedTiles[position] = tileData;

            // replace this with the correct tile later
            TileBase selectedTileSprite = tileData.TileList[Random.Range(0, tileData.TileList.Count)];
            _tilemap.SetTile(position, selectedTileSprite);

            // Set the correct color
            Color tileColor = tileData.TileColor;
            _tilemap.SetTileFlags(position, TileFlags.None);
            _tilemap.SetColor(position, tileColor);
            _tilemap.SetTileFlags(position, TileFlags.LockColor);
        }

        private void Remove1WideWaterTilesAround(Vector3Int position)
        {
            if (!_generatedTiles.ContainsKey(position))
            {
                return;
                throw new Exception("This method should only be called on existing tiles");
            }
            if (_generatedTiles[position] != _waterTile)
            {
                return;
            }

            if ((TileAt(position + Vector3Int.right) != _waterTile) && (TileAt(position + Vector3Int.left) != _waterTile))
            {
                SetTileAt(position, _groundTile);
                Remove1WideWaterTilesAround(position + Vector3Int.up);
                Remove1WideWaterTilesAround(position + Vector3Int.down);
            }
            if ((TileAt(position + Vector3Int.up) != _waterTile) && (TileAt(position + Vector3Int.down) != _waterTile))
            {
                SetTileAt(position, _groundTile);
                Remove1WideWaterTilesAround(position + Vector3Int.right);
                Remove1WideWaterTilesAround(position + Vector3Int.left);
            }
        }

        private void Generate()
        {
            _tilemap.ClearAllTiles();
            _generatedTiles.Clear();

            for (int i = -_generationRange; i < _generationRange + 1; i++)
            {
                for (int j = -_generationRange; j < _generationRange + 1; j++)
                {
                    // Create the tile
                    Vector3Int position = new Vector3Int(i, j);
                    GenerateAt(position);
                }
            }
        }
    }

    // Custom property drawer for the MapGenerator class
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MapGenerator mapGenerator = (MapGenerator)target;

            if (GUILayout.Button("Generate Map"))
            {
                MethodInfo generateMethodInfo = typeof(MapGenerator).GetMethod("Generate", BindingFlags.NonPublic | BindingFlags.Instance);
                generateMethodInfo?.Invoke(mapGenerator, null);    
            }
        }
    }
}
