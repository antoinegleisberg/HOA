using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildPreviewGameState : BaseGameState
{
    private GameObject _previewBuilding;
    private string _previewBuildingName;
    [SerializeField] private GameObject _previewCanvas;
    private List<Tile> _highlightedTiles;
    private bool _positionIsValid;

    public override void EnterState() { }

    public override void Init() {
        _previewCanvas = GameObject.Find("PreviewCanvas");
        _highlightedTiles = new List<Tile>();
    }

    public void Preview(string buildingName)
    {
        _previewBuilding = BuildingUtilities.GetPreviewBuilding(
            buildingName,
            InputManager.instance.mouseWorldPosition,
            Camera.main.transform.rotation,
            _previewCanvas.transform);
        _previewBuildingName = buildingName;
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

            List<Vector2Int> tilePositions = BuildingUtilities.GetOccupiedPositions(mousePosition, _previewBuildingName);

            _positionIsValid = true;
            foreach (Vector2Int position in tilePositions) { 
                Tile tile = GridManager.instance.GetTile(position);
                if (tile != null)
                {
                    _highlightedTiles.Add(tile);
                    Color color = tile.IsOccupied() ? Color.red : Color.green;
                    if (tile.IsOccupied()) _positionIsValid = false; 
                    tile.Highlight(color);
                }
                else { _positionIsValid = false; }
            }
            if (_positionIsValid) { _previewBuilding.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f); }
            else { _previewBuilding.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f); }
        }
    }

    public override void HandleClickOnTile(Vector3 clickCoordinates)
    {
        if (!_positionIsValid) return;
        GridManager.instance.BuildBuilding(_previewBuildingName, clickCoordinates);
        UIEvents.instance.ExitPreview();
    }

    public override void OnDestroy() { }
}
