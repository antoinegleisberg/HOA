using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildPreviewGameState : BaseGameState
{
    private GameObject _previewBuilding;
    private Vector2Int _previewBuildingSize;
    [SerializeField] private GameObject _previewCanvas;
    private List<Tile> _highlightedTiles;

    public override void EnterState() { GameEvents.instance.EnterPreviewBuildingGameState(); }

    public override void Init() {
        _previewCanvas = GameObject.Find("PreviewCanvas");
        _highlightedTiles = new List<Tile>();
    }

    public void Preview(string buildingName)
    {
        _previewBuilding = BuildingsManager.instance.GetPreviewBuilding(
            buildingName,
            InputManager.instance.mouseWorldPosition,
            Camera.main.transform.rotation,
            _previewCanvas.transform);
        _previewBuildingSize = BuildingsManager.instance.GetBuildingByName(buildingName).size;
    }

    public override void ExitState()
    {
        foreach (Tile tile in _highlightedTiles) { tile.DeactivateHightlight(); }
        _highlightedTiles.Clear();
        GameObject.Destroy(_previewBuilding);
        GameEvents.instance.ExitPreviewBuildingGameState();
    }

    public override void UpdateState()
    {
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        foreach (Tile tile in _highlightedTiles) { tile.DeactivateHightlight(); }
        _highlightedTiles.Clear();
        if (_previewBuilding != null)
        {
            Vector3 mousePosition = InputManager.instance.mouseWorldPosition;
            _previewBuilding.transform.position = mousePosition;

            Vector2Int centerPosition = new Vector2Int();
            centerPosition.x = _previewBuildingSize.x % 2 == 0 ? (int)Mathf.Round(mousePosition.x) : (int)Mathf.Floor(mousePosition.x);
            centerPosition.y = _previewBuildingSize.y % 2 == 0 ? (int)Mathf.Round(mousePosition.y) : (int)Mathf.Floor(mousePosition.y);
            for (int x = -_previewBuildingSize.x / 2; x < _previewBuildingSize.x / 2 + 0.5; x++)
            {
                for (int y = -_previewBuildingSize.y / 2; y < _previewBuildingSize.y / 2 + 0.5; y++)
                {
                    Tile tile = GridManager.instance.GetTile(centerPosition + new Vector2Int(x, y));
                    if (tile != null)
                    {
                        _highlightedTiles.Add(tile);
                        Color color = tile.IsOccupied() ? Color.red : Color.green;
                        tile.Highlight(color);
                    }
                }
            }
        }
    }

    public override void HandleClickOnTile(Vector3 coordinates) { }

    public override void OnDestroy() { }
}
