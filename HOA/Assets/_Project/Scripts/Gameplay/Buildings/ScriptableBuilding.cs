using UnityEngine;


namespace antoinegleisberg.HOA
{
    [CreateAssetMenu(fileName ="NewBuidling", menuName ="ScriptableObjects/Building")]
    public class ScriptableBuilding : ScriptableObject
    {
        public string Name;
        public Vector2Int Size;
        public Building BuildingPrefab;
    }
}
