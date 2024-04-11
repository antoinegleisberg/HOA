using antoinegleisberg.HOA.Saving;
using UnityEditor;
using UnityEngine;
using antoinegleisberg.Saving;
using System.Reflection;


namespace antoinegleisberg.HOA.Editor
{
    public static class EditorScripts
    {
        [MenuItem("HOA Scripts/Saving/Create Preset")]
        public static void CreatePreset()
        {
            string name = "Preset Name";
            string directoryName = "preset-name";
            PresetInfo presetInfo = new PresetInfo()
            {
                PresetName = name,
                DirectoryName = directoryName
            };

            string presetFileName = "presets/" + directoryName + "/preset.json";
            string gameDataFileName = "presets/" + directoryName + "/game_data.json";

            SaveSystem.SaveObject(presetFileName, presetInfo);
            SaveSystem.SaveObject(gameDataFileName, null);
        }

        [MenuItem("HOA Scripts/Saving/Reset Saves Infos")]
        public static void ResetSavesInfos()
        {
            FieldInfo field = typeof(SaveFilesManager).GetField("__saveInfos", BindingFlags.NonPublic | BindingFlags.Static);
            field.SetValue(null, null);
        }

        [MenuItem("HOA Scripts/Gameplay/Spawn Citizen")]
        public static void SpawnCitizen()
        {
            Object obj = Object.FindAnyObjectByType(typeof(UnitManager));
            if (obj == null) return;
            if (!Application.IsPlaying(obj)) return;

            Debug.Log("Spawning citizen");

            UnitManager.Instance.SpawnCitizen(Vector3.zero);
        }

        [MenuItem("HOA Scripts/Gameplay/Build All Construction Sites")]
        public static void BuildAllConstructionSites()
        {
            Object obj = Object.FindAnyObjectByType(typeof(BuildingsDB));
            if (obj == null) return;
            if (!Application.IsPlaying(obj)) return;

            foreach (Building building in BuildingsDB.Instance.GetAllBuildings())
            {
                if (building.IsConstructionSite)
                {
                    BuildingsBuilder.Instance.BuildBuilding(building.GetComponent<ConstructionSite>());
                }
            }
        }
    }
}
