using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName = "TileType", menuName = "ScriptableObjects/Tiles/TileType")]
    public class TileType : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        
        [field: SerializeField] public bool IsWalkable { get; private set; }

        [field: SerializeField] public bool IsBuildable { get; private set; }

        [field: SerializeField] public bool CanSpawnResourceSite { get; private set; }

        public static bool operator ==(TileType a, TileType b) => a.Name == b.Name;
        public static bool operator !=(TileType a, TileType b) => a.Name != b.Name;
        public override bool Equals(object obj) => obj is TileType data && data.Name == Name;
        public override int GetHashCode() => Name.GetHashCode();
    }
}
