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

    public abstract class AGameLevel : SerializedMonoBehaviour, IDeletable
    {
        #region Fields
        #region Serialized
        /// <summary>
        /// The current GameLevelData of this GameLevel.
        /// </summary>
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [Tooltip("The current GameLevelData of this GameLevel.")]
        [SerializeField] protected AGameLevelData aGameLevelData = null;
        #endregion Serialized

        #region Internals
        /// <summary>
        /// All the modules added when the game level is build.
        /// </summary>
        private List<IGameLevelModule> _gameLevelModules = null;

        /// <summary>
        /// The cancellation token used to stop async methods when the object is destroy.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource = null;

        /// <summary>
        /// The current GameLevel instance.
        /// </summary>
        private static AGameLevel _current = null;
        
#if UNITY_EDITOR
        /// <summary>
        /// A flag if the game level is already loaded when the game is launch, useful in editor play mode.
        /// </summary>
        private bool _editor_isGameLevelAlreadyLoaded = false;
#endif
        #endregion Internals

        #region Statics
        /// <summary>
        /// <inheritdoc cref="_current"/>
        /// </summary>
        public static AGameLevel Current => _current;
        #endregion Statics
        #endregion Fields

        #region Methods
        #region LifeCycle
        protected virtual void Awake()
        {
#if UNITY_EDITOR
            if (GameInstance.Instance.IsReady == false)
            {
                _editor_isGameLevelAlreadyLoaded = true;
            }
#endif
            
            _current = this;
            _cancellationTokenSource = new CancellationTokenSource();
            _gameLevelModules = new List<IGameLevelModule>();
            
            BuildGameLevelModules();
        }

        protected virtual async void Start()
        {
#if UNITY_EDITOR
            if (_editor_isGameLevelAlreadyLoaded)
            {
                while (GameInstance.Instance.IsReady == false)
                {
                    await Task.Yield();
                }
                
                GameLevelManager.Instance.EditorSetAsAlreadyLoadedLevel(aGameLevelData);
            }
#endif
        }

        private void OnDestroy()
        {
            Delete();
        }

        public virtual void Delete()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }

            if (_gameLevelModules.IsNullOrEmpty() == false)
            {
                _gameLevelModules.Clear();
                _gameLevelModules = null;
            }

            if (_gameLevelModules.IsNullOrEmpty() == false)
            {
                foreach (var gameLevelModule in _gameLevelModules)
                {
                    gameLevelModule.Delete();
                }
                _gameLevelModules.Clear();
                _gameLevelModules = null;
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
            
            var gameLevelModulesCount = _gameLevelModules.Count;
            for (var i = 0; i < gameLevelModulesCount; i++)
            {
                var index = i;
                var onLoading = new Action(() =>
                {
                    gameLevelLoadingInfo.LoadingProgressPercent = (index + currentSceneLoadableProgression) / gameLevelModulesCount;
                    progress.Report(gameLevelLoadingInfo);
                });
                
                var loading = _gameLevelModules[i].Load(cancellationToken, onLoading, progression);
                await loading;

                gameLevelLoadingInfo.LoadingProgressPercent = (i + 1f) / gameLevelModulesCount;
                progress.Report(gameLevelLoadingInfo);
            }

            gameLevelLoadingInfo.LoadingProgressPercent = 1f;
            progress.Report(gameLevelLoadingInfo);
            
            GameInstance.Instance.GameInstanceEvents.OnLevelLoaded();
        }
        #endregion Public Methods

        #region Internal Methods
        /// <summary>
        /// All the module of the game level must be added here.
        /// </summary>
        protected abstract void BuildGameLevelModules();
        
        /// <summary>
        /// Add new game level module in the <see cref="BuildGameLevelModules"/> methods to describe the Game Level. After that, the 
        /// </summary>
        protected void AddGameLevelModule<T>(GameLevelModuleBase<T> gameLevelModuleBase) where T : GameLevelModuleData
        {
            _gameLevelModules.Add(gameLevelModuleBase);
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
