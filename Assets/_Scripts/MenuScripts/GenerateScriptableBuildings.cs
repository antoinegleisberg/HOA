using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateScriptableBuildings
{
    [MenuItem("My scripts/Generate scriptable buildings")]
    static void GenerateBuildings()
    {
        foreach (Sprite sprite in Resources.LoadAll<Sprite>("BuildingSprites"))
        {
            string buildingName = char.ToUpper(sprite.name[0]) + sprite.name.Substring(1).ToLower();

            // Create PreviewPrefab
            string previewPrefabPath = $"Assets/Prefabs/Buildings/Preview/{buildingName}.prefab";
            GameObject PreviewPrefab = null;
            if (AssetDatabase.AssetPathToGUID(previewPrefabPath, AssetPathToGUIDOptions.OnlyExistingAssets) == "")
            {
                Debug.Log($"Creating preview prefab of {buildingName}");
                PreviewPrefab = new GameObject(buildingName);
                SpriteRenderer previewRenderer = PreviewPrefab.AddComponent<SpriteRenderer>();
                previewRenderer.sprite = sprite;
                previewRenderer.color = new Color(1, 1, 1, 0.5f);
                PreviewPrefab = PrefabUtility.SaveAsPrefabAsset(PreviewPrefab, previewPrefabPath);
            }

            // Create BuildingPrefab
            string buildingPrefabPath = $"Assets/Prefabs/Buildings/Building/{buildingName}.prefab";
            if (AssetDatabase.AssetPathToGUID(buildingPrefabPath, AssetPathToGUIDOptions.OnlyExistingAssets) == "")
            {
                Debug.Log($"Creating building prefab of {buildingName}");
                GameObject BuildingPrefab = new GameObject(buildingName);
                SpriteRenderer buildingRenderer = BuildingPrefab.AddComponent<SpriteRenderer>();
                buildingRenderer.sprite = sprite;
                BuildingPrefab.AddComponent<DummyBuilding>();
                PrefabUtility.SaveAsPrefabAsset(BuildingPrefab, buildingPrefabPath);
            }

            // Create ScriptableBuilding
            string scriptableBuildingPath = $"Assets/Resources/Buildings/{buildingName}.asset";
            if (AssetDatabase.AssetPathToGUID(scriptableBuildingPath, AssetPathToGUIDOptions.OnlyExistingAssets) == "")
            {
                Debug.Log($"Creating scriptable building {buildingName}");
                ScriptableBuilding scriptableBuilding = ScriptableObject.CreateInstance<ScriptableBuilding>();
                scriptableBuilding.name = buildingName;
                scriptableBuilding.buildingName = buildingName;
                scriptableBuilding.size = new Vector2Int(Mathf.RoundToInt(sprite.bounds.size.x), Mathf.RoundToInt(sprite.bounds.size.y));
                scriptableBuilding.PreviewPrefab = PreviewPrefab;
                AssetDatabase.CreateAsset(scriptableBuilding, scriptableBuildingPath);
            }
        }
    }
}
