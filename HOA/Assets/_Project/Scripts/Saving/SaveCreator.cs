using System.Linq;
using antoinegleisberg.Saving;

namespace antoinegleisberg.HOA.Saving
{
    internal static class SaveCreator
    {
        public static SaveInfo CreateNewSave(string saveName, string presetName)
        {
            string directoryName = SaveNameManager.BuildDirectoryName(saveName);
            string presetDirectoryName = PresetFilesManager.GetPresets().Where(preset => preset.PresetName == presetName).First().DirectoryName;

            SaveNameManager.CreateSaveDirectory(directoryName);

            SaveFileInfo saveInfo = new SaveFileInfo { SaveName = saveName, DirectoryName = directoryName };

            object gamePreset = SaveSystem.LoadObject(PresetNameManager.AbsolutePresetGameDataPath(presetDirectoryName));
            
            SaveSystem.SaveObject(SaveNameManager.AbsoluteSaveDataPath(directoryName), saveInfo);
            SaveSystem.SaveObject(SaveNameManager.AbsoluteGameDataPath(directoryName), gamePreset);
            
            SaveFilesManager.AddSave(saveName, saveInfo);

            return saveInfo.ToSaveInfo();
        }
    }
}