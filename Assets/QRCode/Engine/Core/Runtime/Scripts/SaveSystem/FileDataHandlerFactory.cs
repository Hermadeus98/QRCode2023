namespace QRCode.Engine.Core.SaveSystem
{
    public static class FileDataHandlerFactory
    {
        public static IFileDataHandler CreateFileDataHandler(string fullPath, string fullFileName)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return new FileDataHandler(fullPath, fullFileName);
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
