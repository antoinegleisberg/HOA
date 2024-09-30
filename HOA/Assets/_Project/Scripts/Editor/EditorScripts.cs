using antoinegleisberg.HOA.Saving;
using UnityEditor;
using UnityEngine;
using antoinegleisberg.Saving;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using antoinegleisberg.HOA.Core;


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

            List<ConstructionSite> constructionSites = BuildingsDB.Instance.GetAllBuildings()
                .Where(b => b.IsConstructionSite)
                .Select(b => b.GetComponent<ConstructionSite>())
                .ToList();

            foreach (ConstructionSite constructionSite in constructionSites)
            {
                BuildingsBuilder.Instance.BuildBuilding(constructionSite);
            }
        }

        [MenuItem("HOA Scripts/Reload Static Classes")]
        public static void ReloadStaticClasses()
        {
            ScriptableBuildingsDB.Reload();
            ScriptableItemsDB.Reload();
            RecipesDB.Reload();
        }
    }
}
