using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using antoinegleisberg.Saving;

[assembly: InternalsVisibleTo("antoinegleisberg.HOA.Editor")]

namespace antoinegleisberg.HOA.Saving
{
    internal static class SaveFilesManager
    {
        private static Dictionary<string, SaveFileInfo> __saveInfos;
        
        private static Dictionary<string, SaveFileInfo> _saveInfos {
            get
            {
                if (__saveInfos == null)
                {
                    LoadSaves();
                }
                return __saveInfos;
            }
        }

        public static void AddSave(string saveName, SaveFileInfo saveInfo)
        {
            _saveInfos.Add(saveName, saveInfo);
        }

        public static List<string> GetSaveNames()
        {
            return _saveInfos.Keys.ToList();
        }

        public static SaveFileInfo GetSave(string saveName)
        {
            if (!SaveExists(saveName))
            {
                throw new Exception("Save does not exist: " + saveName);
            }
            return _saveInfos[saveName];
        }

        public static bool SaveExists(string saveName)
        {
            return _saveInfos.ContainsKey(saveName);
        }

        private static void LoadSaves()
        {
            __saveInfos = new Dictionary<string, SaveFileInfo>();

            DirectoryInfo dirInfo = new DirectoryInfo(SaveNameManager.SavesDirectory);

            DirectoryInfo[] directories = dirInfo.GetDirectories();

            foreach (DirectoryInfo directory in directories)
            {
                SaveFileInfo saveInfo = LoadSaveInfo(directory.Name);
                __saveInfos.Add(saveInfo.SaveName, saveInfo);
            }
        }

        private static SaveFileInfo LoadSaveInfo(string saveDirectoryName)
        {
            string savePath = SaveNameManager.AbsoluteSaveDataPath(saveDirectoryName);

            object saveInfo = SaveSystem.LoadObject(savePath);

            return (SaveFileInfo)saveInfo;
        }
    }
}