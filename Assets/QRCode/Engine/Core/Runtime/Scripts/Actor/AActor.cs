namespace QRCode.Engine.Core.Actor
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.GameInstance;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Debugging;

    /// <summary>
    /// An <see cref="AActor"/> is a tangible object in the game composed of <see cref="AActorModule"/>.
    /// </summary>
    public abstract class AActor : IGameplayComponent
    {
        #region Fields
        private List<AActorModule> _allActorModules = null;
        private AActorSceneReference _actorSceneReference = null;
        #endregion Fields

        #region Properties
        /// <summary>
        /// All <see cref="AActorModule"/> that's compose the <see cref="AActor"/>.
        /// </summary>
        public List<AActorModule> AllActorModules { get { return _allActorModules; } }
        
        /// <summary>
        /// The <see cref="_actorSceneReference"/> of the actor.
        /// </summary>
        protected AActorSceneReference ActorSceneReference { get { return _actorSceneReference; } }
        #endregion Properties

        #region Constructor
        public AActor()
        {
            _allActorModules = new List<AActorModule>();
        }
        #endregion Constructor

        #region Methods
        #region LifeCycle
        /// <summary>
        /// Initialization of the <see cref="AActor"/>.
        /// </summary>
        public async Task InitAsync(AActorModule[] actorModules, AActorSceneReference actorSceneReference)
        {
            _actorSceneReference = actorSceneReference;
            
            int modulesCount = actorModules.Length;
            for (int i = 0; i < modulesCount; i++)
            {
                await AddModuleInternal(actorModules[i]);
            }
        }
        
        public virtual void Delete()
        {
            GameInstance.Instance.GameInstanceEvents.UnregisterGameplayComponent(this);

            int modulesCount = _allActorModules.Count;
            for (int i = 0; i < modulesCount; i++)
            {
                _allActorModules[i].Delete();
            }
        }
        #endregion LifeCycle

        #region Public Methods
        /// <summary>
        /// Add a <see cref="AActorModule"/> to the <see cref="AActor"/>.
        /// </summary>
        /// <typeparam name="t_module"></typeparam>
        public async Task AddModule<t_module>() where t_module : AActorModule, new()
        {
            t_module actorModule = new AActorModule() as t_module;
            await AddModuleInternal(actorModule);
        }

        /// <summary>
        /// Try to remove a <see cref="AActorModule"/> of a specific type.
        /// </summary>
        public bool TryRemoveFirstModuleOfType<t_module>() where t_module : AActorModule
        {
            int modulesCount = _allActorModules.Count;
            for (int i = 0; i < modulesCount; i++)
            {
                if (_allActorModules[i].GetType() == typeof(t_module))
                {
                    return TryRemoveModule(_allActorModules[i]);
                }
            }

            QRLogger.DebugError<CoreTags.Actor>($"Cannot remove module of type {typeof(t_module)} in {nameof(_allActorModules)}.");
            return false;
        }

        /// <summary>
        /// Try to remove a <see cref="AActorModule"/> by reference.
        /// </summary>
        public bool TryRemoveModule(AActorModule actorModule)
        {
            if (_allActorModules.Contains(actorModule))
            {
                _allActorModules.Remove(actorModule);
                actorModule.Delete();
                return true;
            }

            QRLogger.DebugError<CoreTags.Actor>($"Cannot remove {nameof(actorModule)} of {this.GetType()} because {nameof(_allActorModules)} don't contains {nameof(actorModule)}");
            return false;
        }

        /// <summary>
        /// Try to get a <see cref="AActorModule"/> of a specific type, return true if the <see cref="AActorModule"/> is found.
        /// </summary>
        public bool TryGetFirstModuleOfType<t_module>(out t_module actorModule) where t_module : AActorModule
        {
            int modulesCount = _allActorModules.Count;
            for (int i = 0; i < modulesCount; i++)
            {
                if (_allActorModules[i].GetType() == typeof(t_module))
                {
                    actorModule = _allActorModules[i] as t_module;
                    return true;
                }
            }

            actorModule = null;
            return false;
        }

        /// <summary>
        /// Get all the <see cref="AActorModule"/> of a type of this <see cref="AActor"/>.
        /// </summary>
        public List<t_module> GetAllModulesOfType<t_module>() where t_module : AActorModule
        {
            List<t_module> modulesList = new List<t_module>();
            
            int modulesCount = _allActorModules.Count;
            for (int i = 0; i < modulesCount; i++)
            {
                if (_allActorModules[i].GetType() == typeof(t_module))
                {
                    modulesList.Add(_allActorModules[i] as t_module);
                }
            }

            return modulesList;
        }
        #endregion Public Methods

        #region Private Methods
        private async Task AddModuleInternal(AActorModule actorModule)
        {
            _allActorModules.Add(actorModule);
            await actorModule.InitActorModuleAsync(this);
        }
        #endregion Private Methods

        #region Callbacks
        public virtual void OnLevelLoaded() { }

        public virtual void OnLevelUnloaded() { }

        public virtual void OnGameInstanceIsReady() { }

        public virtual void OnGameStart() { }

        public virtual void OnGameUpdate() { }

        public virtual void OnGameEnd() { }

        public virtual void OnGameRestart() { }

        public virtual void OnGamePause(PauseInfo pauseInfo) { }
        #endregion Callbacks
        #endregion Methods
    }
}
