using antoinegleisberg.Types;
using System.Collections.Generic;
using UnityEngine;


namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName ="NewBuidling", menuName ="ScriptableObjects/Buildings/Building")]
    public class ScriptableBuilding : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }

        [TextArea(3, 10)]
        [SerializeField] private string _description;
        public string Description => _description;

        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public Vector2Int Size { get; private set; }
        [field: SerializeField] public Building BuildingPrefab { get; private set; }
        [field: SerializeField] public List<Pair<ScriptableItem, int>> BuildingMaterials { get; private set; }
    }
}
