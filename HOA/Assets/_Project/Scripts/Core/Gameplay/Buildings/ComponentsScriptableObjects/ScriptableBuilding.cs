using antoinegleisberg.Types;
using System.Collections.Generic;
using UnityEngine;


namespace antoinegleisberg.HOA
{
    [CreateAssetMenu(fileName ="NewBuidling", menuName ="ScriptableObjects/Buildings/Building")]
    public class ScriptableBuilding : ScriptableObject
    {
        public string Name;
        [TextArea(3, 10)]
        public string Description;
        public Sprite Sprite;
        public Vector2Int Size;
        public Building BuildingPrefab;
        public List<Pair<ScriptableItem, int>> BuildingMaterials;
    }
}
