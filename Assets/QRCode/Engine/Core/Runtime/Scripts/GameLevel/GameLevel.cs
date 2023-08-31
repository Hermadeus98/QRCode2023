namespace QRCode.Engine.Core.GameLevels
{
    using UnityEngine;

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Sirenix.OdinInspector;
    using Sirenix.Utilities;
    
    using QRCode.Engine.Toolbox;
    using QRCode.Engine.Core.GameInstance;
    using QRCode.Engine.Toolbox.Optimization;

    public abstract class GameLevel : SerializedMonoBehaviour, IGameLevel, IDeletable
    {
        #region Fields
        #region Serialized
        /// <summary>
        /// The current GameLevelData of this GameLevel.
        /// </summary>
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [Tooltip("The current GameLevelData of this GameLevel.")]
        [SerializeField] protected GameLevelData m_gameLevelData = null;
        #endregion Serialized

        #region Internals
        /// <summary>
        /// All the modules added when the game level is build.
        /// </summary>
        private List<IGameLevelModule> m_gameLevelModules = null;
        
        /// <summary>
        /// The scene loading info of the game level.
        /// </summary>
        private SceneLoadingInfo m_sceneLoadingInfo;
        
        /// <summary>
        /// A flag if the game level is already loaded when the game is launch, useful in editor play mode.
        /// </summary>
        private bool m_isGameLevelAlreadyLoaded = false;
        
        /// <summary>
        /// The cancellation token used to stop async methods when the object is destroy.
        /// </summary>
        private CancellationTokenSource m_cancellationTokenSource = null;
        #endregion Internals

        #region Statics
        /// <summary>
        /// The current GameLevel instance;
        /// </summary>
        public static GameLevel Current = null;
        #endregion Statics
        #endregion Fields

        #region Methods
        #region LifeCycle
        protected virtual void Awake()
        {
            if (GameInstance.Instance.IsReady == false)
            {
                m_isGameLevelAlreadyLoaded = true;
            }
            
            Current = this;
            m_cancellationTokenSource = new CancellationTokenSource();
            m_gameLevelModules = new List<IGameLevelModule>();
            
            BuildGameLevelModules();
        }

        protected virtual async void Start()
        {
            if (m_isGameLevelAlreadyLoaded)
            {
                while (GameInstance.Instance.IsReady == false)
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
        }
        #endregion LifeCycle

        #region Public Methods
        /// <summary>
        /// Function call when the game level is unload by the <see cref="GameLevelManager"/>.
        /// </summary>
        public void UnloadLevel()
        {
            GameInstance.Instance.GameInstanceEvents.OnLevelUnloaded();    
        }
        
        /// <summary>
        /// Function call when the game level is load by the <see cref="GameLevelManager"/>.
        /// </summary>
        public async Task LoadLevel(CancellationToken cancellationToken, IProgress<GameLevelLoadingInfo> progress)
        {
            var gameLevelLoadingInfo = new GameLevelLoadingInfo();
            
            var currentSceneLoadableProgression = 0f;
            var progression = new Progress<float>(value =>
            {
                currentSceneLoadableProgression = value;
            });
            
            var gameLevelModulesCount = m_gameLevelModules.Count;
            for (var i = 0; i < gameLevelModulesCount; i++)
            {
                var index = i;
                var onLoading = new Action(() =>
                {
                    gameLevelLoadingInfo.LoadingProgressPercent = (index + currentSceneLoadableProgression) / gameLevelModulesCount;
                    progress.Report(gameLevelLoadingInfo);
                });
                
                var loading = m_gameLevelModules[i].Load(cancellationToken, onLoading, progression);
                await loading;

                gameLevelLoadingInfo.LoadingProgressPercent = (i + 1f) / gameLevelModulesCount;
                progress.Report(gameLevelLoadingInfo);
            }

            gameLevelLoadingInfo.LoadingProgressPercent = 1f;
            progress.Report(gameLevelLoadingInfo);
            
            GameInstance.Instance.GameInstanceEvents.OnLevelLoaded();
        }
        
        /// <summary>
        /// All the module of the game level must be added here.
        /// </summary>
        public abstract void BuildGameLevelModules();
        #endregion Public Methods

        #region Internal Methods
        /// <summary>
        /// Add new game level module in the <see cref="BuildGameLevelModules"/> methods to describe the Game Level. After that, the 
        /// </summary>
        protected void AddGameLevelModule<T>(GameLevelModuleBase<T> gameLevelModuleBase) where T : GameLevelModuleData
        {
            m_gameLevelModules.Add(gameLevelModuleBase);
        }
        #endregion Internal Methods
        #endregion Methods
    }
    
    /// <summary>
    /// All loading info when a level is loaded.
    /// </summary>
    public struct GameLevelLoadingInfo
    {
        public float LoadingProgressPercent;
    }
}
