using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace antoinegleisberg.HOA.Saving
{
    public static class SaveManager
    {
        private static readonly string SAVES_DIRECTORY = "saves";
        private static readonly string SAVE_PATH = Application.persistentDataPath + "/" + SAVES_DIRECTORY;
        private static readonly string SAVE_DATA_FILE_NAME = "save.json";
        private static readonly string GAME_DATA_FILE_NAME = "game_data.json";

        private static readonly string PRESETS_DIRECTORY = "presets";
        private static readonly string PRESETS_PATH = Application.persistentDataPath + "/" + PRESETS_DIRECTORY;
        private static readonly string PRESET_DATA_FILE_NAME = "preset.json";
        private static readonly string PRESET_GAME_DATA_FILE_NAME = "game_data.json";

        private static Dictionary<string, SaveFileInfo> __saveInfos;

        private static Dictionary<string, SaveFileInfo> _saveInfos {
            get {
                if (__saveInfos == null)
                {
                    LoadSaves();
                }
                return __saveInfos;
            }
        }

        private struct SaveFileInfo
        {
            public string SaveName;
            public string DirectoryName;

            public SaveInfo ToSaveInfo()
            {
                return new SaveInfo { SaveName = SaveName };
            }
        }

        public static SaveNameValidation IsValidNewSaveName(string saveName)
        {
            if (string.IsNullOrEmpty(saveName))
            {
                return new SaveNameValidation { Success = false, ErrorMessage = "You cannot have an empty save name." };
            }

            if (_saveInfos.ContainsKey(saveName))
            {
                return new SaveNameValidation { Success = false, ErrorMessage = "A save with this name already exists!" };
            }

            string directoryName = BuildDirectoryName(saveName);

            if (string.IsNullOrEmpty(directoryName))
            {
                return new SaveNameValidation { Success = false, ErrorMessage = "Your save name has to contain at least one alphanumeric character." };
            }

            if (Directory.Exists(SAVE_PATH + "/" + directoryName))
            {
                return new SaveNameValidation { Success = false, ErrorMessage = "A save with the same alphanumeric characters already exists!" };
            }

            return new SaveNameValidation { Success = true, ErrorMessage = "" };
        }

        public static SaveInfo GetSave(string saveName)
        {
            if (!SaveExists(saveName))
            {
                throw new Exception("Save does not exist");
            }
            return _saveInfos[saveName].ToSaveInfo();
        }

        public static SaveInfo CreateNewSave(string saveName, string presetDirectoryName)
        {
            string directoryName = BuildDirectoryName(saveName);

            Directory.CreateDirectory(SAVE_PATH + "/" + directoryName);

            SaveFileInfo saveInfo = new SaveFileInfo { SaveName = saveName, DirectoryName = directoryName };

            object gamePreset = SaveSystem.SaveSystem.LoadObject(PRESETS_DIRECTORY + "/" + presetDirectoryName + "/" + PRESET_GAME_DATA_FILE_NAME);

            SaveSystem.SaveSystem.SaveObject(SAVE_PATH + "/" + directoryName + "/" + SAVE_DATA_FILE_NAME, saveInfo);
            SaveSystem.SaveSystem.SaveObject(SAVE_PATH + "/" + directoryName + "/" + GAME_DATA_FILE_NAME, gamePreset);

            _saveInfos.Add(saveName, saveInfo);

            return saveInfo.ToSaveInfo();
        }

        public static List<string> GetSaveNames()
        {
            return _saveInfos.Keys.ToList();
        }

        public static SaveInfo GetSaveInfo(string saveName)
        {
            if (!_saveInfos.ContainsKey(saveName))
            {
                throw new Exception("Save file does not exist: " + saveName);
            }
            return _saveInfos[saveName].ToSaveInfo();
        }

        public static string GetRelativeGameDataPath(string saveName)
        {
            return SAVES_DIRECTORY + "/" + _saveInfos[saveName].DirectoryName + "/" + GAME_DATA_FILE_NAME;
        }

        public static bool SaveExists(string saveName)
        {
            return _saveInfos.ContainsKey(saveName);
        }

        public static List<PresetInfo> GetPresets()
        {
            List<PresetInfo> presets = new List<PresetInfo>();

            if (!Directory.Exists(PRESETS_PATH))
            {
                Directory.CreateDirectory(PRESETS_PATH);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(PRESETS_PATH);

            DirectoryInfo[] directories = dirInfo.GetDirectories();

            foreach (DirectoryInfo directory in directories)
            {
                PresetInfo presetInfo = GetPresetInfo(directory.Name);
                presets.Add(presetInfo);
            }

            return presets;
        }

        private static PresetInfo GetPresetInfo(string directoryName)
        {
            string relativePresetFilePath = PRESETS_DIRECTORY + "/" + directoryName + "/" + PRESET_DATA_FILE_NAME;

            return (PresetInfo)SaveSystem.SaveSystem.LoadObject(relativePresetFilePath);
        }

        private static string BuildDirectoryName(string saveName)
        {
            return Regex.Replace(saveName, "[^a-zA-Z0-9]", "");
        }

        private static void LoadSaves()
        {
            __saveInfos = new Dictionary<string, SaveFileInfo>();

            if (!Directory.Exists(SAVE_PATH))
            {
                Directory.CreateDirectory(SAVE_PATH);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(SAVE_PATH);

            DirectoryInfo[] directories = dirInfo.GetDirectories();

            foreach (DirectoryInfo directory in directories)
            {
                SaveFileInfo saveInfo = LoadSaveInfo(directory.Name);
                __saveInfos.Add(saveInfo.SaveName, saveInfo);
            }
        }

        private static SaveFileInfo LoadSaveInfo(string directoryName)
        {
            string savePath = SAVE_PATH + "/" + directoryName + "/" + SAVE_DATA_FILE_NAME;

            object saveInfo = SaveSystem.SaveSystem.LoadObject(savePath);

            return (SaveFileInfo)saveInfo;
        }
    }
}
