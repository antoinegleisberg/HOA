using antoinegleisberg.Saving;
using System;
using System.Collections.Generic;

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
            public bool IsConstructionSite;
            public float[] WorldPosition;
            public string Guid;
            public Dictionary<string, int> Items;

            public BuildingSaveData(Building building)
            {
                WorldPosition = new float[3] { building.transform.position.x, building.transform.position.y, building.transform.position.z };
                BuildingName = building.ScriptableBuilding.Name;
                IsConstructionSite = building.IsConstructionSite;
                Guid = building.GetComponent<GuidHolder>().UniqueId;
                Items = new Dictionary<string, int>();
                foreach (KeyValuePair<ScriptableItem, int> kvp in building.GetComponent<Storage>().Items())
                {
                    Items.Add(kvp.Key.Name, kvp.Value);
                }
            }
        }
    }
}