namespace antoinegleisberg.HOA
{
    public partial class BuildingsDB
    {
        [System.Serializable]
        private struct BuildingsDBSaveData
        {
            public BuildingSaveData[] Buildings;
        }
        
        
        [System.Serializable]
        private struct BuildingSaveData
        {
            public string BuildingName;
            public float[] WorldPosition;
            public string Guid;
        }
    }
}