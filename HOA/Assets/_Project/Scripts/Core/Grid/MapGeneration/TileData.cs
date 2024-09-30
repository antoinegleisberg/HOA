using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/Tiles/TileData")]
    public class TileData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        
        // Not used for now: I will do that later once I actually have the sprites for the tiles
        [SerializeField] private List<TileBase> tileList;
        public IReadOnlyList<TileBase> TileList => tileList;

        // Temporary: will be removed once I have the sprites for the tiles
        [field: SerializeField] public Color TileColor { get; private set; }

        [field: SerializeField] public TileType TileType { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }

        public static bool operator ==(TileData a, TileData b) => a.Name == b.Name;
        public static bool operator !=(TileData a, TileData b) => a.Name != b.Name;
        public override bool Equals(object obj) => obj is TileData data && data.Name == Name;
        public override int GetHashCode() => Name.GetHashCode();
    }
}
