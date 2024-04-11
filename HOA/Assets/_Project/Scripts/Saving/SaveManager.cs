using System.Collections.Generic;

namespace antoinegleisberg.HOA.Saving
{
    public static class SaveManager
    {
        public static SaveNameValidation IsValidNewSaveName(string saveName)
        {
            return SaveNameValidator.IsValidNewSaveName(saveName);
        }

        public static SaveInfo GetSave(string saveName)
        {
            return SaveFilesManager.GetSave(saveName).ToSaveInfo();
        }
        
        public static List<PresetInfo> GetPresets()
        {
            return PresetFilesManager.GetPresets();
        }

        public static SaveInfo CreateNewSave(string saveName, string presetName)
        {
            return SaveCreator.CreateNewSave(saveName, presetName);
        }

        public static List<string> GetSaveNames()
        {
            return SaveFilesManager.GetSaveNames();
        }

        public static string GetRelativeGameDataPath(string saveName)
        {
            string directoryName = SaveFilesManager.GetSave(saveName).DirectoryName;

            return SaveNameManager.RelativeGameDataPath(directoryName);
        }
    }
}
