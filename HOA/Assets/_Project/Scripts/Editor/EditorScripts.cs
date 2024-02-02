using antoinegleisberg.HOA.Saving;
using UnityEditor;
using UnityEngine;


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

            SaveSystem.SaveSystem.SaveObject(presetFileName, presetInfo);
            SaveSystem.SaveSystem.SaveObject(gameDataFileName, "");
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
    }
}
