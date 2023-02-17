namespace QRCode.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class Initialization : SerializedMonoBehaviour
    {
        [SerializeField] private List<ISceneLoadable> m_sceneLoadables = new List<ISceneLoadable>();
        
        public static Initialization Current = null;
        
        private void Awake()
        {
            Current = this;
        }

        public async Task Load(CancellationToken cancellationToken, IProgress<SceneLoadableProgressionInfos> progress)
        {
            var sceneLoadableProgressionInfos = new SceneLoadableProgressionInfos();
            
            var currentSceneLoadableProgression = 0f;
            var progression = new Progress<float>(value =>
            {
                currentSceneLoadableProgression = value;
            });
            
            var sceneLoadableCount = m_sceneLoadables.Count;
            for (var i = 0; i < sceneLoadableCount; i++)
            {
                var index = i;
                var onLoading = new Action(() =>
                {
                    sceneLoadableProgressionInfos.LoadingProgressPercent = (index + currentSceneLoadableProgression) / sceneLoadableCount;
                    sceneLoadableProgressionInfos.ProgressionDescription = m_sceneLoadables[i].SceneLoadableProgressionInfos.ProgressionDescription;
                    progress.Report(sceneLoadableProgressionInfos);
                });
                
                var loading = m_sceneLoadables[i].Load(cancellationToken, onLoading, progression);
                await loading;

                sceneLoadableProgressionInfos.LoadingProgressPercent = (i + 1f) / sceneLoadableCount;
                progress.Report(sceneLoadableProgressionInfos);
            }

            sceneLoadableProgressionInfos.LoadingProgressPercent = 1f;
            progress.Report(sceneLoadableProgressionInfos);
        }
    }
}
