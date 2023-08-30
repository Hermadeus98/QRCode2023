﻿namespace QRCode.Engine.Core.SaveSystem
{
    using UnityEngine;

    using System.Linq;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Sirenix.OdinInspector;

    using Managers;
    using Debugging;
    using Toolbox;
    using Toolbox.Optimization;
    using Toolbox.Pattern.Singleton;
    using Constants = Toolbox.Constants;

    public class SaveManager : MonoBehaviourSingleton<SaveManager>, ISaveService, IManager, IDeletable
    {
        #region Fields
        [TitleGroup(Constants.InspectorGroups.Debugging)]
        [ShowInInspector] private GameData m_gameData = null;
        
        private IFileDataHandler m_fileDataHandler = null;
        private SaveServiceSettings m_saveServiceSettings = null;
        private CancellationTokenSource m_cancellationTokenSource = null;

        private bool m_isSaving = false;
        private bool m_isLoading = false;
        private bool m_isInit = false;
        #endregion

        #region Events
        private Action m_onStartSave;
        private Action m_onEndSave;
        private Action m_onStartLoad;
        private Action m_onEndLoad;
        
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
        public async Task InitAsync(CancellationToken cancellationToken)
        {
            if(m_isInit)
                return;
            
            var saveSystemSettings = SaveServiceSettings.Instance;
            m_fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveSystemSettings.FullPath, saveSystemSettings.FullFileName);
            m_saveServiceSettings = SaveServiceSettings.Instance;
            m_cancellationTokenSource = new CancellationTokenSource();

#if UNITY_STANDALONE
            Application.wantsToQuit += OnStandaloneExit;
#endif

            await LoadGameAsync();
            m_isInit = true;
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
            QRDebug.Debug(Constants.DebuggingChannels.SaveManager, $"New game data was created...");
            return Task.CompletedTask;
        }

        [Button]
        public async Task LoadGameAsync()
        {
            if (m_isLoading)
            {
                QRDebug.DebugError(Constants.DebuggingChannels.SaveManager,$"Game is already loading...");
                return;
            }
            
            m_isLoading = true;
            m_onStartLoad?.Invoke();

            m_gameData = await m_fileDataHandler.Load<GameData>();

            if (m_gameData == null)
            {
                QRDebug.Debug(Constants.DebuggingChannels.SaveManager, $"No {nameof(m_gameData)} was found. Initializing default values.");
                await NewGame();
            }   
            
            Load.Current.LoadObjects();
            m_onEndLoad?.Invoke();
            m_isLoading = false;

            QRDebug.Debug(Constants.DebuggingChannels.SaveManager,$"Game is load.");
        }

        [Button]
        public async Task SaveGameAsync()
        {
            if (m_isSaving)
            {
                QRDebug.DebugError(Constants.DebuggingChannels.SaveManager,$"Game is already saving...");
                return;
            }   
            
            m_isSaving = true;
            m_onStartSave?.Invoke();
            
            if (m_saveServiceSettings.UseFakeSave)
            {
                await Task.Delay(TimeSpan.FromSeconds(m_saveServiceSettings.FakeSaveDuration), m_cancellationTokenSource.Token);
            }
            
            Save.Current.SaveObjects();
            await m_fileDataHandler.Save(m_gameData);
            m_onEndSave?.Invoke();
            m_isSaving = false;
            
            QRDebug.Debug(Constants.DebuggingChannels.SaveManager,$"Game is save.");
        }

        [Button]
        public async Task DeleteSave()
        {
            var task = m_fileDataHandler.TryDeleteSave();
            await task;

            if (task.Result == true)
            {
                QRDebug.Debug(Constants.DebuggingChannels.SaveManager, $"Save is delete.");
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
        
        public bool IsInit()
        {
            return m_isInit;
        }
        
        public void Delete()
        {
            m_gameData = null;
            if (m_cancellationTokenSource != null)
            {
                m_cancellationTokenSource.Cancel();
                m_cancellationTokenSource.Dispose();
                m_cancellationTokenSource = null;
            }

            m_fileDataHandler = null;
        }
        
        #endregion

        #region Lifecycle
        private void OnDestroy()
        {
            Delete();
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
                QRDebug.DebugError(Constants.DebuggingChannels.Editor, $"There is no Game Data");
                return null;
            }
                
            var loadableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ILoadableObject>().ToArray();

            for (var i = 0; i < loadableObjects.Length; i++)
            {
                loadableObjects[i].LoadGameData(gameData as GameData);
            }
                
            QRDebug.Debug(Constants.DebuggingChannels.Editor, $"Load in editor is successful.");
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
            var fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveSystemSettings.FullPath,
                saveSystemSettings.FullFileName);
            await fileDataHandler.Save(gameData);
            
            QRDebug.Debug(Constants.DebuggingChannels.Editor, $"Save in editor is successful.");
        }
        
        public static void DeleteSaveInEditor()
        {
            var saveSystemSettings = SaveServiceSettings.Instance;
            var fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveSystemSettings.FullPath,
                saveSystemSettings.FullFileName);
            fileDataHandler.TryDeleteSave();
            QRDebug.Debug(Constants.DebuggingChannels.Editor, $"Save Data has been deleted successfully");
        }
#endif
        #endregion
        #endregion
    }
}