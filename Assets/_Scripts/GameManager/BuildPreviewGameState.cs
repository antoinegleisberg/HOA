using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildPreviewGameState : BaseGameState
{
    private GameObject _previewBuilding;
    [SerializeField] private GameObject _previewCanvas;

    public override void EnterState() { GameEvents.instance.EnterPreviewBuildingGameState(); }

    public override void Init() { _previewCanvas = GameObject.Find("PreviewCanvas"); }

    public void Preview(string buildingName)
    {
        _previewBuilding = BuildingsManager.instance.GetPreviewBuilding(
            buildingName,
            InputManager.instance.mouseWorldPosition,
            Camera.main.transform.rotation,
            _previewCanvas.transform);
    }

    public override void ExitState()
    {
        GameObject.Destroy(_previewBuilding);
        GameEvents.instance.ExitPreviewBuildingGameState();
    }

    public override void UpdateState()
    {
        if (_previewBuilding != null)
        {
            _previewBuilding.transform.position = InputManager.instance.mouseWorldPosition;
        }
    }

    public override void HandleClickOnTile(Vector3 coordinates) { }

    public override void OnDestroy() { }
}
