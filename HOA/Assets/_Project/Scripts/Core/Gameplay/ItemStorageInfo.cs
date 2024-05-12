using System.Collections.Generic;

namespace antoinegleisberg.HOA.Core
{
    public struct ItemStorageInfo
    {
        public ScriptableItem Item;
        public Dictionary<Storage, int> Availability; 
    }
}