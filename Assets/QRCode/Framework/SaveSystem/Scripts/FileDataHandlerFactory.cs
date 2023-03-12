namespace QRCode.Framework
{
    public static class FileDataHandlerFactory
    {
        public static IFileDataHandler CreateFileDataHandler()
        {
            var saveSystemSettings = SaveServiceSettings.Instance;
            
#if UNITY_EDITOR
            return new FileDataHandler(saveSystemSettings.FullPath, saveSystemSettings.FullFileName);
#else
            
            //#if PS
            
            //#elif XBOX
            
            //#elif SWITCH
            
            //PC
            //#else
            return new FileDataHandler(saveSystemSettings.FullPath, saveSystemSettings.FullFileName);
            //#endif
#endif
        }
    }
}
