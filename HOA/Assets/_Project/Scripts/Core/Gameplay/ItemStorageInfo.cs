using System.Collections.Generic;

namespace antoinegleisberg.HOA
{
    public struct ItemStorageInfo
    {
        public ScriptableItem Item;
        public Dictionary<Storage, int> Availability; 
    }
}