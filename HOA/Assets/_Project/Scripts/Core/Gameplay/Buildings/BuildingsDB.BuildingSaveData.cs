using System;

namespace antoinegleisberg.HOA.Core
{
    public partial class BuildingsDB
    {
        [Serializable]
        private struct BuildingsDBSaveData
        {
            public BuildingSaveData[] Buildings;
        }
        
        
        [Serializable]
        private struct BuildingSaveData
        {
            public string BuildingName;
            public float[] WorldPosition;
            public string Guid;
        }
    }
}