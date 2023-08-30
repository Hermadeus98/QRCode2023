namespace QRCode.Engine.Core.SceneManagement
{
    using UnityEngine.ResourceManagement.AsyncOperations;
    
    public struct SceneLoadingInfo
    {
        private DownloadStatus m_downloadStatus;

        public DownloadStatus DownloadStatus
        {
            get
            {
                return m_downloadStatus;
            }
        }

        public SceneLoadingInfo(DownloadStatus downloadStatus)
        {
            m_downloadStatus = downloadStatus;
        }
    }
}
