using antoinegleisberg.Types;
using System.Collections.Generic;
using UnityEngine;


namespace antoinegleisberg.HOA
{
    [CreateAssetMenu(fileName ="NewBuidling", menuName ="ScriptableObjects/Building")]
    public class ScriptableBuilding : ScriptableObject
    {
        public string Name;
        public Vector2Int Size;
        public Building BuildingPrefab;
        public List<Pair<ScriptableItem, int>> BuildingMaterials;
    }
}
