namespace antoinegleisberg.HOA.Saving
{
    internal struct SaveFileInfo
    {
        public string SaveName;
        public string DirectoryName;

        public SaveInfo ToSaveInfo()
        {
            return new SaveInfo { SaveName = SaveName };
        }
    }
}
