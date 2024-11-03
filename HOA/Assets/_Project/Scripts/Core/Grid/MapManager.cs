using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    /// <summary>
    /// In charge of holding the information of generated tiles, saving player changes
    /// as well as triggering tile generation and display
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private MapDisplayManager _mapDisplayManager;
        [SerializeField] private ResourceSiteGenerator _resourceSiteGenerator;

        private Dictionary<Vector3Int, TileData> _generatedTiles;

        // Controls the initial generation range
        [SerializeField] private int _generationRange = 50;

        private void Awake()
        {
            Instance = this;
            _generatedTiles = new Dictionary<Vector3Int, TileData>();
        }

        private void Start()
        {
            Generate();
            _mapDisplayManager.UpdateDisplay(_generatedTiles.Keys.ToList());
        }

        public bool IsTileGenerated(Vector3Int position)
        {
            return _generatedTiles.ContainsKey(position);
        }

        public TileData GetTileAt(Vector3Int position)
        {
            if (!_generatedTiles.ContainsKey(position))
            {
                _mapGenerator.GenerateAt(position);
            }
            return _generatedTiles[position];
        }

        public void SetTileAt(Vector3Int position, TileData tileData)
        {
            _generatedTiles[position] = tileData;
            _mapDisplayManager.UpdateTileDisplay(position, tileData);
            _resourceSiteGenerator.CheckForResourceSiteGeneration(position);
        }

        public ResourceSite GetResourceSiteAt(Vector3Int position)
        {
            return _resourceSiteGenerator.GetResourceSiteAt(position);
        }

        private void Generate()
        {
            _mapDisplayManager.ResetDisplay();
            _generatedTiles.Clear();

            for (int i = -_generationRange; i < _generationRange + 1; i++)
            {
                for (int j = -_generationRange; j < _generationRange + 1; j++)
                {
                    Vector3Int position = new Vector3Int(i, j);
                    _mapGenerator.GenerateAt(position);
                }
            }
        }
    }

    // Custom property drawer for the MapGenerator class
    [CustomEditor(typeof(MapManager))]
    public class MapGeneratorDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MapManager mapGenerator = (MapManager)target;

            if (GUILayout.Button("Generate Map"))
            {
                MethodInfo generateMethodInfo = typeof(MapManager).GetMethod("Generate", BindingFlags.NonPublic | BindingFlags.Instance);
                generateMethodInfo?.Invoke(mapGenerator, null);
                MethodInfo updateDisplayMethodInfo = typeof(MapManager).GetMethod("UpdateDisplay", BindingFlags.NonPublic | BindingFlags.Instance);
                updateDisplayMethodInfo?.Invoke(mapGenerator, null);
            }
        }
    }
}
