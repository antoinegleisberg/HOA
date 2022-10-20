using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPreviewGameState : BaseGameState
{
    private GameObject _previewObject;
    private Dictionary<string, GameObject> _spritePrefabs;
    [SerializeField] private GameObject _previewCanvas;

    public override void EnterState() { }

    public override void Init()
    {
        _previewCanvas = GameObject.Find("PreviewCanvas");
        Debug.Log(_previewCanvas);
        _spritePrefabs = new Dictionary<string, GameObject>();
        foreach (GameObject prefab in Resources.LoadAll("BuildingPreviewPrefabs")) {
            _spritePrefabs[prefab.name] = prefab;
        }
    }

    public void Preview(string name)
    {
        Debug.Log(Resources.LoadAll("BuildingPreviewPrefabs"));
        foreach (KeyValuePair<string, GameObject> kvp in _spritePrefabs)
        {
            Debug.Log(kvp.Key);
            Debug.Log(kvp.Value);
        }
        _previewObject = GameObject.Instantiate(_spritePrefabs[name], InputManager.instance.mouseWorldPosition, Camera.main.transform.rotation, _previewCanvas.transform);
        _previewObject.name = name;
    }

    public override void ExitState()
    {
        GameObject.Destroy(_previewObject);
    }

    public override void UpdateState()
    {
        if (_previewObject != null)
        {
            _previewObject.transform.position = InputManager.instance.mouseWorldPosition;
        }
    }

    public override void HandleClickOnTile(Vector3 coordinates)
    {

    }
}
