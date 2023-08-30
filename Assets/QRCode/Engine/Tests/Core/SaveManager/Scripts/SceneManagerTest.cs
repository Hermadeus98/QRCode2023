namespace QRCode.Engine.Core.SceneManagement.Tests
{
    using System;
    using GeneratedEnum;
    using Sirenix.OdinInspector;
    
    using UnityEngine;

    public class SceneManagerTest : MonoBehaviour
    {
        public DB_ScenesEnum scene;

        private Progress<SceneLoadingInfo> m_sceneLoadingProgress = null;

        [Button]
        public async void LoadScene()
        {
            m_sceneLoadingProgress = new Progress<SceneLoadingInfo>();
            m_sceneLoadingProgress.ProgressChanged += (sender, data) =>
            {
                Debug.Log(data.DownloadStatus.Percent);
                Debug.Log(data.DownloadStatus.DownloadedBytes);
            };
            
            await SceneManager.Instance.LoadScene(scene, m_sceneLoadingProgress);
        }

        [Button]
        public async void UnloadScene()
        {
            await SceneManager.Instance.UnLoadScene(scene);
        }
    }
}
