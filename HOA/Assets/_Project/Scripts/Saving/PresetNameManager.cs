using System.IO;
using UnityEngine;


namespace antoinegleisberg.HOA.Saving
{
    internal static class PresetNameManager
    {
        private static readonly string PRESETS_DIRECTORY = "presets";
        private static readonly string PRESETS_PATH = Application.persistentDataPath + "/" + PRESETS_DIRECTORY;
        private static readonly string PRESET_DATA_FILE_NAME = "preset.json";
        private static readonly string PRESET_GAME_DATA_FILE_NAME = "game_data.json";

        public static string PresetsDirectory {
            get
            {
                if (!Directory.Exists(PRESETS_PATH))
                {
                    Directory.CreateDirectory(PRESETS_PATH);
                }
                return PRESETS_PATH;
            }
        }

        public static string AbsolutePresetsPath()
        {
            return PRESETS_PATH;
        }

        public static string AbsolutePresetGameDataPath(string presetDirectoryName)
        {
            return PresetsDirectory + "/" + presetDirectoryName + "/" + PRESET_GAME_DATA_FILE_NAME;
        }

        public static string AbsolutePresetDataPath(string presetDirectoryName)
        {
            return PresetsDirectory + "/" + presetDirectoryName + "/" + PRESET_DATA_FILE_NAME;
        }
    }
}
