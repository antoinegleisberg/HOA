using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace antoinegleisberg.HOA
{
    public class PreviewManager : MonoBehaviour
    {
        public static PreviewManager Instance { get; private set; }

        [SerializeField] private Camera _camera;
        [SerializeField] private Grid _grid;
        [SerializeField] private Tilemap _floorTilemap;
        [SerializeField] private Tilemap _greenHighlightTilemap;
        [SerializeField] private Tilemap _redHighlightTilemap;
        [SerializeField] private Tile _tile;

        private bool _isPreviewing;
        private bool _currentPositionIsValid;
        public ScriptableBuilding PreviewBuilding { get; private set; }
        [SerializeField] private SpriteRenderer _previewBuildingSR;


        private void Awake()
        {
            Instance = this;
            _previewBuildingSR.enabled = false;
        }

        private void Update()
        {
            if (!_isPreviewing)
            {
                return;
            }

            UpdatePreview();
        }

        public void StartPreview(string name)
        {
            _isPreviewing = true;
            _previewBuildingSR.enabled = true;
            _currentPositionIsValid = false;

            PreviewBuilding = BuildingsDB.GetBuildingByName(name);
            _previewBuildingSR.sprite = PreviewBuilding.BuildingPrefab.GetComponent<SpriteRenderer>().sprite;

            UpdatePreview();
        }

        public void CancelPreview()
        {
            _isPreviewing = false;
            _previewBuildingSR.enabled = false;
            _currentPositionIsValid = false;

            PreviewBuilding = null;
            _greenHighlightTilemap.ClearAllTiles();
            _redHighlightTilemap.ClearAllTiles();
        }

        public bool CurrentPositionIsValid()
        {
            return _currentPositionIsValid;
        }

        private void UpdatePreview()
        {

            Vector2 mousePos = InputManager.Instance.MouseScreenPosition;

            Vector3 worldPosition = _camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));

            Vector3 localPos = _grid.WorldToLocal(worldPosition);

            Vector3 localInterpolated = _grid.LocalToCellInterpolated(localPos);

            _previewBuildingSR.transform.position = worldPosition;

            List<Vector2Int> occupiedTiles = BuildingsPlacer.GetOccupiedTiles(localInterpolated, PreviewBuilding.Size);

            _greenHighlightTilemap.ClearAllTiles();
            _redHighlightTilemap.ClearAllTiles();
            _currentPositionIsValid = true;
            foreach (Vector2Int tilePos in occupiedTiles)
            {
                if (GridManager.Instance.TileIsOccupied(tilePos))
                {
                    _redHighlightTilemap.SetTile(new Vector3Int(tilePos.x, tilePos.y), _tile);
                    _currentPositionIsValid = false;
                }
                else
                {
                    _greenHighlightTilemap.SetTile(new Vector3Int(tilePos.x, tilePos.y), _tile);
                }
            }

            // Debug.Log("Mouse position: " + mousePos + " | World position: " + worldPosition + " | Local pos: " + localPos + " | Grid pos: " + gridPos + " | Local interpolated: " + localInterpolated);
        }
    }
}
