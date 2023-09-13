namespace QRCode.Engine.Core.SaveSystem
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Sirenix.OdinInspector;
    
    using QRCode.Engine.Core.Manager;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Toolbox.Optimization;
    using Constants = QRCode.Engine.Toolbox.Constants;
    
    using UnityEngine;

    public class SaveManager : GenericManagerBase<SaveManager>, IDeletable
    {
        #region Fields
        #region Serialized
        [TitleGroup(Constants.InspectorGroups.References)][ReadOnly]
        [Tooltip("The current loaded game data.")]
        [ShowInInspector] private GameData m_gameData = null;
        #endregion Serialized

        #region Internals
        private IFileDataHandler m_fileDataHandler = null;
        private SaveServiceSettings m_saveServiceSettings = null;

        private bool m_isSaving = false;
        private bool m_isLoading = false;
        #endregion Internals
        #endregion

        #region Events
        private Action m_onStartSave = null;
        private Action m_onEndSave = null;
        private Action m_onStartLoad = null;
        private Action m_onEndLoad = null;
        
        public event Action OnStartSave
        {
            add => m_onStartSave += value;
            remove => m_onStartSave -= value;
        }

        public event Action OnEndSave
        {
            add => m_onEndSave += value;
            remove => m_onEndSave -= value;
        }
        
        public event Action OnStartLoad
        {
            add => m_onStartLoad += value;
            remove => m_onStartLoad -= value;
        }
        
        public event Action OnEndLoad
        {
            add => m_onEndLoad += value;
            remove => m_onEndLoad -= value;
        }
        #endregion

        #region Methods
        #region Initialization
        protected override async Task InitAsync(CancellationToken cancellationToken)
        {
            var saveSystemSettings = SaveServiceSettings.Instance;
            m_fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveSystemSettings.FullPath, saveSystemSettings.FullFileName);
            m_saveServiceSettings = SaveServiceSettings.Instance;

#if UNITY_STANDALONE
            Application.wantsToQuit += OnStandaloneExit;
#endif

            await LoadGameAsync();
        }
        #endregion

        #region Publics
        public GameData GetGameData()
        {
            return m_gameData;
        }
        
        public Task NewGame()
        {
            m_gameData = new GameData();
            QRLogger.Debug<CoreTags.Save>($"New game data was created...");
            return Task.CompletedTask;
        }

        [Button]
        public async Task LoadGameAsync()
        {
            if (m_isLoading)
            {
                QRLogger.DebugError<CoreTags.Save>($"Game is already loading...");
                return;
            }
            
            m_isLoading = true;
            m_onStartLoad?.Invoke();

            m_gameData = await m_fileDataHandler.Load<GameData>();

            if (m_gameData == null)
            {
                QRLogger.Debug<CoreTags.Save>($"No {nameof(m_gameData)} was found. Initializing default values.");
                await NewGame();
            }   
            
            Load.Current.LoadObjects();
            m_onEndLoad?.Invoke();
            m_isLoading = false;

            QRLogger.Debug<CoreTags.Save>($"Game data is load.");
        }

        [Button]
        public async Task SaveGameAsync()
        {
            if (m_isSaving)
            {
                QRLogger.DebugError<CoreTags.Save>($"Game is already saving...");
                return;
            }   
            
            m_isSaving = true;
            m_onStartSave?.Invoke();
            
            if (m_saveServiceSettings.UseFakeSave)
            {
                await Task.Delay(TimeSpan.FromSeconds(m_saveServiceSettings.FakeSaveDuration), CancellationTokenSource.Token);
            }
            
            Save.Current.SaveObjects();
            await m_fileDataHandler.Save(m_gameData);
            m_onEndSave?.Invoke();
            m_isSaving = false;
            
            QRLogger.Debug<CoreTags.Save>($"Game data is save.");
        }

        [Button]
        public async Task DeleteSave()
        {
            var task = m_fileDataHandler.TryDeleteSave();
            await task;

            if (task.Result == true)
            {
                QRLogger.Debug<CoreTags.Save>($"Game data is delete.");
            }
        }

        public bool IsSaving()
        {
            return m_isSaving;
        }

        public bool IsLoading()
        {
            return m_isLoading;
        }

        public override void Delete()
        {
            if (m_gameData != null)
            {
                m_gameData.Delete();
                m_gameData = null;
            }

            if (m_fileDataHandler != null)
            {
                m_fileDataHandler.Dispose();
                m_fileDataHandler = null;
            }
        }
        
        #endregion

        #region Privates
        private bool OnStandaloneExit()
        {
            Application.wantsToQuit -= OnStandaloneExit;
            if (m_isSaving)
            {
                StandaloneQuitAfterSave(10);
                return false;
            }

            return true;
        }
        
        private async void StandaloneQuitAfterSave(int a_timeout)
        {
            var time = 0f;
            while (m_isSaving && time < a_timeout)
            {
                await Task.Yield();
                time += Time.deltaTime;
            }

            Delete();
            Application.Quit();
        }
        #endregion

        #region Editor
#if UNITY_EDITOR
        public static async Task<GameData> LoadInEditor()
        {
            var saveSystemSettings = SaveServiceSettings.Instance;
            var fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveSystemSettings.FullPath,
                saveSystemSettings.FullFileName);
            var gameData = await fileDataHandler.Load<GameData>();

            if (gameData == null)
            {
                QRLogger.DebugError<CoreTags.Save>($"There is no Game Data");
                return null;
            }
                
            var loadableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ILoadableObject>().ToArray();

            for (var i = 0; i < loadableObjects.Length; i++)
            {
                loadableObjects[i].LoadGameData(gameData as GameData);
            }
                
            QRLogger.Debug<CoreTags.Save>($"Load in editor is successful.");
            return gameData as GameData;
        }

        public static async void SaveInEditor()
        {
            var gameData = new GameData();
            var savableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISavableObject>().ToArray();

            for (var i = 0; i < savableObjects.Length; i++)
            {
                savableObjects[i].SaveGameData(ref gameData);
            }
            
            var saveSystemSettings = SaveServiceSettings.Instance;
            var fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveSystemSettings.FullPath, saveSystemSettings.FullFileName);
            await fileDataHandler.Save(gameData);
            
            QRLogger.Debug<CoreTags.Save>($"Save in editor is successful.");
        }
        
        public static void DeleteSaveInEditor()
        {
            var saveSystemSettings = SaveServiceSettings.Instance;
            var fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveSystemSettings.FullPath, saveSystemSettings.FullFileName);
            fileDataHandler.TryDeleteSave();
            QRLogger.Debug<CoreTags.Save>($"Save Data has been deleted successfully");
        }
#endif
        #endregion
        #endregion
    }
}