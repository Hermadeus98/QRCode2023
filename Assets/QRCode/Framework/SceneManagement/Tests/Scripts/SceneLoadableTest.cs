namespace QRCode.Framework
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class SceneLoadableTest : SerializedMonoBehaviour, ISceneLoadable
    {
        [TitleGroup(K.InspectorGroups.Settings)]
        [SerializeField] private float m_waitDuration = 1f;

        [TitleGroup(K.InspectorGroups.Debugging)]
        [SerializeField] private SceneLoadableProgressionInfos m_sceneLoadableProgressionInfos;
        [SerializeField][ReadOnly] private bool m_isLoaded = false;

        public SceneLoadableProgressionInfos SceneLoadableProgressionInfos
        {
            get => m_sceneLoadableProgressionInfos; 
            set => m_sceneLoadableProgressionInfos = value;
        }

        public async Task Load(CancellationToken cancellationToken, Action onLoading, IProgress<float> progress)
        {
            var elapsedTime = 0f;
            
            while (elapsedTime < m_waitDuration)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                elapsedTime += Time.deltaTime;
                if (elapsedTime > m_waitDuration) elapsedTime = m_waitDuration;

                onLoading?.Invoke();

                var progressionPercent = elapsedTime / m_waitDuration;
                
                var sceneLoadableProgressionInfos = SceneLoadableProgressionInfos;
                sceneLoadableProgressionInfos.LoadingProgressPercent = progressionPercent;
                
                progress.Report(progressionPercent);
                
                await Task.Yield();
            }
            
            progress.Report(1f);
            m_isLoaded = true;
        }
    }
}
