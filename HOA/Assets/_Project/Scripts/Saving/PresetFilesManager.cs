using System.Collections.Generic;
using System.IO;
using antoinegleisberg.Saving;

namespace antoinegleisberg.HOA.Saving
{
    internal static class PresetFilesManager
    {
        public static List<PresetInfo> GetPresets()
        {
            List<PresetInfo> presets = new List<PresetInfo>();

            DirectoryInfo dirInfo = new DirectoryInfo(PresetNameManager.AbsolutePresetsPath());

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
            string relativePresetFilePath = PresetNameManager.AbsolutePresetDataPath(directoryName);

            return (PresetInfo)SaveSystem.LoadObject(relativePresetFilePath);
        }
    }
}
