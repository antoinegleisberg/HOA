using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Scriptable Objects/Building")]
public class ScriptableBuilding : ScriptableObject
{
    public string buildingName;
    public Vector2Int size;
    public BaseBuilding BuildingPrefab;
    public GameObject PreviewPrefab;
}
