using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using antoinegleisberg.HOA.Input;
using antoinegleisberg.HOA.Core;
using System.Collections;
using antoinegleisberg.Types;

namespace antoinegleisberg.HOA.Preview
{
    public class PreviewManager : MonoBehaviour
    {
        public static PreviewManager Instance { get; private set; }
        
        [SerializeField] private Tilemap _floorTilemap;
        [SerializeField] private Tilemap _greenHighlightTilemap;
        [SerializeField] private Tilemap _redHighlightTilemap;
        [SerializeField] private Tile _tile;

        private bool _isPreviewing;
        public bool CurrentPositionIsValid { get; private set; }
        
        public ScriptableBuilding PreviewBuilding { get; private set; }
        [SerializeField] private SpriteRenderer _previewBuildingSR;


        private void Awake()
        {
            Instance = this;
            _previewBuildingSR.enabled = false;
        }

        public void StartPreview(string name)
        {
            _isPreviewing = true;
            _previewBuildingSR.enabled = true;
            CurrentPositionIsValid = false;

            PreviewBuilding = ScriptableBuildingsDB.GetBuildingByName(name);
            _previewBuildingSR.sprite = PreviewBuilding.BuildingPrefab.ScriptableBuilding.Sprite;

            StartCoroutine(UpdatePreviewCoroutine());
        }

        public void CancelPreview()
        {
            _isPreviewing = false;
            _previewBuildingSR.enabled = false;
            CurrentPositionIsValid = false;

            PreviewBuilding = null;
            _greenHighlightTilemap.ClearAllTiles();
            _redHighlightTilemap.ClearAllTiles();
        }

        private IEnumerator UpdatePreviewCoroutine()
        {
            while (_isPreviewing)
            {
                UpdatePreview();
                yield return null;
            }
        }
        
        private bool CanBuildOnTile(Vector2Int tilePos)
        {
            return !GridManager.Instance.TileIsOccupied(tilePos) && MapManager.Instance.GetTileAt(tilePos.ToVector3Int()).TileType.IsBuildable;
        }

        private void UpdatePreview()
        {
            Vector2 mousePos = InputManager.Instance.MouseScreenPosition;

            Vector3 worldPosition = GridManager.Instance.MouseToWorldPosition(mousePos);

            _previewBuildingSR.transform.position = worldPosition;

            Vector3 localInterpolated = GridManager.Instance.MouseToInterpolatedCellPosition(mousePos);

            List<Vector2Int> occupiedTiles = BuildingsPlacer.GetOccupiedTiles(localInterpolated, PreviewBuilding.Size);

            _greenHighlightTilemap.ClearAllTiles();
            _redHighlightTilemap.ClearAllTiles();
            CurrentPositionIsValid = true;
            foreach (Vector2Int tilePos in occupiedTiles)
            {
                if (!CanBuildOnTile(tilePos))
                {
                    _redHighlightTilemap.SetTile(new Vector3Int(tilePos.x, tilePos.y), _tile);
                    CurrentPositionIsValid = false;
                }
                else
                {
                    _greenHighlightTilemap.SetTile(new Vector3Int(tilePos.x, tilePos.y), _tile);
                }
            }
        }
    }
}
