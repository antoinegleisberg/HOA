using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingsManager : MonoBehaviour
{
    public static BuildingsManager instance;

    private Dictionary<string, ScriptableBuilding> _buildings;

    private void Awake()
    {
        instance = this;

        _buildings = new Dictionary<string, ScriptableBuilding>();
        foreach (ScriptableBuilding building in Resources.LoadAll<ScriptableBuilding>("Buildings"))
        {
            _buildings[building.buildingName] = building;
        }
    }

    public void BuildBuilding(string buildingName, Vector2 coordinates)
    {

    }

    public GameObject GetPreviewBuilding(string buildingName, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject previewBuilding = Instantiate(
            _buildings[buildingName].PreviewPrefab,
            position,
            rotation,
            parent);
        previewBuilding.name = buildingName;
        return previewBuilding;
    }
}