namespace QRCode.Engine.Core.GameLevel
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Toolbox;
    using Sirenix.OdinInspector;
    using UI.LoadingScreen;
    using UnityEngine;

    public class GameLevelInitialization : SerializedMonoBehaviour
    {
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [SerializeField] private GameLevelReferenceGroup m_gameLevelReferenceGroup;
        
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [SerializeField] private List<IGameLevelLoadable> m_sceneLoadables = new List<IGameLevelLoadable>();

        private bool m_levelIsLoaded = false;
        private CancellationTokenSource m_cancellationTokenSource = null;
        private ILoadingScreen m_loadingScreen = null;
        private SceneLoadingInfo m_sceneLoadingInfo;
        private bool m_isAlreadyLoaded = false;

        public static GameLevelInitialization Current = null;
        
        private void Awake()
        {
            if (GameInstance.GameInstance.Instance.IsReady == false)
            {
                m_isAlreadyLoaded = true;
            }
            
            Current = this;
            m_cancellationTokenSource = new CancellationTokenSource();
        }

        private async void Start()
        {
            if (m_isAlreadyLoaded)
            {
                while (GameInstance.GameInstance.Instance.IsReady == false)
                {
                    await Task.Yield();
                }
                
                GameLevelManager.Instance.SetAsAlreadyLoadedLevel(m_gameLevelReferenceGroup);
            }
        }

        private void OnDestroy()
        {
            m_cancellationTokenSource.Cancel();
        }

        public void UnloadLevel()
        {
            GameInstance.GameInstance.Instance.GameInstanceEvents.OnLevelUnloaded();    
        }
        
        public async Task LoadLevel(CancellationToken cancellationToken, IProgress<GameLevelLoadProgressionInfos> progress)
        {
            var sceneLoadableProgressionInfos = new GameLevelLoadProgressionInfos();
            
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
                    sceneLoadableProgressionInfos.ProgressionDescription = m_sceneLoadables[i].GameLevelLoadProgressionInfos.ProgressionDescription;
                    progress.Report(sceneLoadableProgressionInfos);
                });
                
                var loading = m_sceneLoadables[i].Load(cancellationToken, onLoading, progression);
                await loading;

                sceneLoadableProgressionInfos.LoadingProgressPercent = (i + 1f) / sceneLoadableCount;
                progress.Report(sceneLoadableProgressionInfos);
            }

            sceneLoadableProgressionInfos.LoadingProgressPercent = 1f;
            progress.Report(sceneLoadableProgressionInfos);
            
            GameInstance.GameInstance.Instance.GameInstanceEvents.OnLevelLoaded();

            m_levelIsLoaded = true;
        }
    }
}
