using System.IO;

namespace antoinegleisberg.HOA.Saving
{
    internal static class SaveNameValidator
    {
        public static SaveNameValidation IsValidNewSaveName(string saveName)
        {
            if (string.IsNullOrEmpty(saveName))
            {
                return new SaveNameValidation { Success = false, ErrorMessage = "You cannot have an empty save name." };
            }

            if (SaveFilesManager.SaveExists(saveName))
            {
                return new SaveNameValidation { Success = false, ErrorMessage = "A save with this name already exists!" };
            }

            string directoryName = SaveNameManager.BuildDirectoryName(saveName);

            if (string.IsNullOrEmpty(directoryName))
            {
                return new SaveNameValidation { Success = false, ErrorMessage = "Your save name has to contain at least one alphanumeric character." };
            }

            if (Directory.Exists(SaveNameManager.AbsoluteSaveDirectoryPath(directoryName)))
            {
                return new SaveNameValidation { Success = false, ErrorMessage = "A save with the same alphanumeric characters already exists!" };
            }

            return new SaveNameValidation { Success = true, ErrorMessage = "" };
        }
    }
}