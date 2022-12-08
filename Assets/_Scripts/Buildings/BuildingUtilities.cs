using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingUtilities
{
    private static Dictionary<string, ScriptableBuilding> _scriptableBuildings;

    private static void Init()
    {
        _scriptableBuildings = new Dictionary<string, ScriptableBuilding>();
        foreach (ScriptableBuilding building in Resources.LoadAll<ScriptableBuilding>("Buildings"))
        {
            _scriptableBuildings[building.buildingName] = building;
        }
    }

    public static GameObject GetPreviewBuilding(string buildingName, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (_scriptableBuildings == null) Init();
        GameObject previewBuilding = GameObject.Instantiate(
            _scriptableBuildings[buildingName].PreviewPrefab,
            position,
            rotation,
            parent);
        previewBuilding.name = buildingName;
        return previewBuilding;
    }

    public static ScriptableBuilding GetBuildingByName(string buildingName) { return _scriptableBuildings[buildingName]; }

    public static List<Vector2Int> GetOccupiedPositions(Vector3 mousePosition, string buildingName)
    {
        Vector2Int size = _scriptableBuildings[buildingName].size;

        List<Vector2Int> occupiedTiles = new List<Vector2Int>();

        Vector2Int centerPosition = new Vector2Int();
        centerPosition.x = size.x % 2 == 0 ? Mathf.FloorToInt(mousePosition.x + 0.5f) : Mathf.FloorToInt(mousePosition.x);
        centerPosition.y = size.y % 2 == 0 ? Mathf.FloorToInt(mousePosition.y + 0.5f) : Mathf.FloorToInt(mousePosition.y);

        for (int x = -size.x / 2; x < 1.0 * size.x / 2; x++)
        {
            for (int y = -size.y / 2; y < 1.0 * size.y / 2; y++)
            {
                occupiedTiles.Add(centerPosition + new Vector2Int(x, y));
            }
        }
        return occupiedTiles;
    }
}