using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace antoinegleisberg.HOA.Saving
{
    internal static class SaveNameManager
    {
        private static readonly string SAVES_DIRECTORY = "saves";
        private static readonly string SAVE_PATH = Application.persistentDataPath + "/" + SAVES_DIRECTORY;
        private static readonly string SAVE_DATA_FILE_NAME = "save.json";
        private static readonly string GAME_DATA_FILE_NAME = "game_data.json";

        public static string SavesDirectory
        {
            get
            {
                if (!Directory.Exists(SAVE_PATH))
                {
                    Directory.CreateDirectory(SAVE_PATH);
                }
                return SAVE_PATH;
            }
        }

        public static string BuildDirectoryName(string saveName)
        {
            return Regex.Replace(saveName, "[^a-zA-Z0-9]", "");
        }

        public static void CreateSaveDirectory(string saveDirectoryName)
        {
            Directory.CreateDirectory(SavesDirectory + "/" + saveDirectoryName);
        }

        public static string AbsoluteSaveDirectoryPath(string saveDirectoryName)
        {
            return SavesDirectory + "/" + saveDirectoryName;
        }

        public static string AbsoluteSaveDataPath(string saveDirectoryName)
        {
            return SavesDirectory + "/" + saveDirectoryName + "/" + SAVE_DATA_FILE_NAME;
        }

        public static string AbsoluteGameDataPath(string saveDirectoryName)
        {
            return SavesDirectory + "/" + saveDirectoryName + "/" + GAME_DATA_FILE_NAME;
        }

        public static string RelativeGameDataPath(string saveDirectoryName)
        {
            return SAVES_DIRECTORY + "/" + saveDirectoryName + "/" + GAME_DATA_FILE_NAME;
        }
    }
}
