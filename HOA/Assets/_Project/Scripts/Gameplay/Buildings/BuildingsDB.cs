using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public static class BuildingsDB
    {
        private static Dictionary<string, ScriptableBuilding> _buildings;

        private static string _path = "Buildings";

        private static void Init()
        {
            _buildings = new Dictionary<string, ScriptableBuilding>();

            ScriptableBuilding[] buildings = Resources.LoadAll<ScriptableBuilding>(_path);
            
            foreach (ScriptableBuilding building in buildings)
            {
                _buildings.Add(building.Name, building);
            }
        }

        public static ScriptableBuilding GetBuildingByName(string name)
        {
            if (_buildings == null)
            {
                Init();
            }

            if (!_buildings.ContainsKey(name))
            {
                Debug.LogError("BuildingsDB does not contain a building with the name " + name);
                return null;
            }

            return _buildings[name];
        }
    }
}
