namespace QRCode.Engine.Core.Actor
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.GameInstance;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Toolbox.Optimization;
    using QRCode.Engine.Toolbox.Pattern.Singleton;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    /// <summary>
    /// The actor factory manage the creation of new actors.
    /// </summary>
    public class ActorFactory : Singleton<ActorFactory>, IDeletable
    {
        #region Fields
        private List<AActor> _allActors = null;
        private Dictionary<Type, List<AActor>> _allActorPerType = null;
        #endregion Fields

        #region Events
        private Action<AActor> _actorCreationCompleted = null;
        
        /// <summary>
        /// This event is call at the end of the actor process creation.
        /// </summary>
        public event Action<AActor> ActorCreationCompleted
        {
            add { _actorCreationCompleted += value; }
            remove { _actorCreationCompleted -= value; }
        }
        #endregion Events

        #region Constructor
        public ActorFactory()
        {
            _allActors = new List<AActor>();
            _allActorPerType = new Dictionary<Type, List<AActor>>();
        }
        #endregion Constructor

        #region Methods
        #region LifeCycle
        public void Delete()
        {
            if (_allActors != null)
            {
                for (int i = 0; i < _allActors.Count; i++)
                {
                    DeleteActor(_allActors[i]);
                }
            }

            _actorCreationCompleted = null;
        }
        #endregion LifeCycle

        #region Public Methods
        /// <summary>
        /// Try to create a new <see cref="AActor"/> into the game world.
        /// </summary>
        public async Task<t_actorType> CreateActor<t_actorType>(ActorCreationParameters actorCreationParameters) where t_actorType : AActor, new()
        {
            t_actorType actor = new t_actorType();
            GameInstance.Instance.GameInstanceEvents.RegisterGameplayComponent(actor);
            RegisterActor(actor);

            var actorGameObjectInstance = await Addressables.InstantiateAsync(actorCreationParameters.ActorGameObjectPrefab).Task;
            
            if (actorGameObjectInstance.TryGetComponent(out AActorSceneReference actorSceneReference) == false)
            {
                QRLogger.DebugError<CoreTags.Actor>($"There no {nameof(AActorSceneReference)} component on {actor}.", actorGameObjectInstance);
                return null;
            }

            Transform actorTransform = actorSceneReference.transform;
            actorTransform.SetParent(actorCreationParameters.ActorParent);
            actorTransform.position = actorCreationParameters.SpawnPosition;
            actorTransform.rotation = actorCreationParameters.SpawnRotation;
            
            actor.Initialize(actorCreationParameters.ActorModules, actorSceneReference);
            actorSceneReference.Initialize(actor);

            _actorCreationCompleted?.Invoke(actor);
            return actor;
        }

        /// <summary>
        /// Delete an <see cref="AActor"/> and free it's memory.
        /// </summary>
        public void DeleteActor(AActor actor)
        {
            GameInstance.Instance.GameInstanceEvents.UnregisterGameplayComponent(actor);
            UnregisterActor(actor);
            actor.Delete();
        }

        /// <summary>
        /// Get all <see cref="AActor"/> of a specific type.
        /// </summary>
        public List<AActor> GetAllActorOfTypes<t_actorType>() where t_actorType : AActor
        {
            Type type = typeof(t_actorType);

            if (_allActorPerType.ContainsKey(type) == false)
            {
                return null;
            }
            
            return _allActorPerType[type];
        }
        
        /// <summary>
        /// Get all <see cref="AActor"/> with a specific <see cref="AActorModule"/>.
        /// </summary>
        public List<AActor> GetAllActorWithModuleOfType<t_moduleType>() where t_moduleType : AActorModule
        {
            List<AActor> allActorWithModuleOfType = new List<AActor>();

            int allActorsCount = _allActors.Count;
            for (int i = 0; i < allActorsCount; i++)
            {
                if (_allActors[i].TryGetFirstModuleOfType<t_moduleType>(out _))
                {
                    allActorWithModuleOfType.Add(_allActors[i]);
                }
            }

            return allActorWithModuleOfType;
        }
        
        /// <summary>
        /// Get all <see cref="AActorModule"/> of a specific type from all <see cref="AActor"/> created.
        /// </summary>
        public List<t_moduleType> GetAllModulesOfTypes<t_moduleType>() where t_moduleType : AActorModule
        {
            List<t_moduleType> allModules = new List<t_moduleType>();

            int actorCount = _allActors.Count;
            for (int i = 0; i < actorCount; i++)
            {
                List<t_moduleType> moduleList = _allActors[i].GetAllModulesOfType<t_moduleType>();
                allModules.AddRange(moduleList);
            }

            return allModules;
        }
        #endregion Public Methods

        #region Private Methods
        private void RegisterActor(AActor actor)
        {
            Type type = actor.GetType();
            
            if (_allActorPerType.ContainsKey(type) == false)
            {
                _allActorPerType.Add(type, new List<AActor>());
            }
            
            _allActorPerType[type].Add(actor);
        }

        private void UnregisterActor(AActor actor)
        {
            Type type = actor.GetType();
            
            if (_allActorPerType.ContainsKey(type))
            {
                _allActorPerType[type].Remove(actor);
            }
        }
        #endregion Private Methods
        #endregion Methods
    }
}
