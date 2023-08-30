namespace QRCode.Engine.Core.GameLevels
{
    using UnityEngine;
    using UnityEngine.Localization;

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Sirenix.OdinInspector;
    using Sirenix.Utilities;
    
    using Toolbox;
    using UI.LoadingScreen;

    public abstract class GameLevel : SerializedMonoBehaviour, IGameLevel
    {
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [SerializeField] protected GameLevelData m_gameLevelData = null;
        
        /// <summary>
        /// Module can be added here, but it's a better way to add them manually with <see cref="BuildGameLevelModules"/>.
        /// </summary>
        private List<IGameLevelModule> m_gameLevelModules = null;
        
        private CancellationTokenSource m_cancellationTokenSource = null;
        private ILoadingScreen m_loadingScreen = null;
        private SceneLoadingInfo m_sceneLoadingInfo;
        private bool m_isAlreadyLoaded = false;

        public static GameLevel Current = null;
        
        protected virtual void Awake()
        {
            if (GameInstance.GameInstance.Instance.IsReady == false)
            {
                m_isAlreadyLoaded = true;
            }
            
            Current = this;
            m_cancellationTokenSource = new CancellationTokenSource();
            m_gameLevelModules = new List<IGameLevelModule>();
            
            BuildGameLevelModules();
        }

        protected virtual async void Start()
        {
            if (m_isAlreadyLoaded)
            {
                while (GameInstance.GameInstance.Instance.IsReady == false)
                {
                    await Task.Yield();
                }
                
                GameLevelManager.Instance.SetAsAlreadyLoadedLevel(m_gameLevelData);
            }
        }

        private void OnDestroy()
        {
            Delete();
        }

        public void UnloadLevel()
        {
            GameInstance.GameInstance.Instance.GameInstanceEvents.OnLevelUnloaded();    
        }
        
        public async Task LoadLevel(CancellationToken cancellationToken, IProgress<GameLevelLoadingInfo> progress)
        {
            var sceneLoadableProgressionInfos = new GameLevelLoadingInfo();
            
            var currentSceneLoadableProgression = 0f;
            var progression = new Progress<float>(value =>
            {
                currentSceneLoadableProgression = value;
            });
            
            var sceneLoadableCount = m_gameLevelModules.Count;
            for (var i = 0; i < sceneLoadableCount; i++)
            {
                var index = i;
                var onLoading = new Action(() =>
                {
                    sceneLoadableProgressionInfos.LoadingProgressPercent = (index + currentSceneLoadableProgression) / sceneLoadableCount;
                    sceneLoadableProgressionInfos.ProgressionDescription = m_gameLevelModules[i].GameLevelLoadingInfo.ProgressionDescription;
                    progress.Report(sceneLoadableProgressionInfos);
                });
                
                var loading = m_gameLevelModules[i].Load(cancellationToken, onLoading, progression);
                await loading;

                sceneLoadableProgressionInfos.LoadingProgressPercent = (i + 1f) / sceneLoadableCount;
                progress.Report(sceneLoadableProgressionInfos);
            }

            sceneLoadableProgressionInfos.LoadingProgressPercent = 1f;
            progress.Report(sceneLoadableProgressionInfos);
            
            GameInstance.GameInstance.Instance.GameInstanceEvents.OnLevelLoaded();
        }

        public void Delete()
        {
            if (m_cancellationTokenSource != null)
            {
                m_cancellationTokenSource.Cancel();
                m_cancellationTokenSource.Dispose();
                m_cancellationTokenSource = null;
            }

            if (m_gameLevelModules.IsNullOrEmpty() == false)
            {
                m_gameLevelModules.Clear();
                m_gameLevelModules = null;
            }

            if (m_gameLevelModules.IsNullOrEmpty() == false)
            {
                foreach (var gameLevelModule in m_gameLevelModules)
                {
                    gameLevelModule.Delete();
                }
                m_gameLevelModules.Clear();
                m_gameLevelModules = null;
            }

            m_loadingScreen = null;
        }

        /// <summary>
        /// All the module of the game level must be added here.
        /// </summary>
        public abstract void BuildGameLevelModules();

        protected void AddGameLevelModule<T>(GameLevelModuleBase<T> gameLevelModuleBase) where T : GameLevelModuleData
        {
            m_gameLevelModules.Add(gameLevelModuleBase);
        }
    }
    
    [Serializable]
    public struct GameLevelLoadingInfo
    {
        [SerializeField] private LocalizedString m_loadingProgressionDescription;
        [SerializeField][ReadOnly] private float m_loadingProgressPercent;

        public float LoadingProgressPercent
        {
            get => m_loadingProgressPercent;
            set => m_loadingProgressPercent = value;
        }
        public LocalizedString ProgressionDescription
        {
            get => m_loadingProgressionDescription;
            set => m_loadingProgressionDescription = value;
        }
    }

}
