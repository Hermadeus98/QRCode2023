using System.Linq;
using UnityEngine;

namespace QRCode.Framework
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Debugging;
    using Sirenix.OdinInspector;

    //https://www.google.com/search?client=firefox-b-d&q=save+system+unity#fpstate=ive&vld=cid:47854c46,vid:aUi9aijvpgs,st:899
    public class SaveService : SerializedMonoBehaviour , ISaveService
    {
        private GameData m_gameData = null;
        private IFileDataHandler m_fileDataHandler = null;
        private SaveServiceSettings m_saveServiceSettings = null;
        private CancellationTokenSource m_cancellationTokenSource = null;

        private bool m_isSaving = false;
        private bool m_isLoading = false;
        private bool m_isInit = false;

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

        public GameData GetGameData()
        {
            return m_gameData;
        }

        public async Task Initialize()
        {
            if(m_isInit)
                return;
            
            var saveSystemSettings = SaveServiceSettings.Instance;
            m_fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveSystemSettings.FullPath, saveSystemSettings.FullFileName);
            m_saveServiceSettings = SaveServiceSettings.Instance;
            m_cancellationTokenSource = new CancellationTokenSource();
            await LoadGameAsync();
            m_isInit = true;
        }

        public Task NewGame()
        {
            m_gameData = new GameData();
            QRDebug.Debug(K.DebuggingChannels.SaveSystem, $"New game data was created...");
            return Task.CompletedTask;
        }

        [Button]
        public async Task LoadGameAsync()
        {
            if (m_isLoading)
            {
                QRDebug.DebugError(K.DebuggingChannels.SaveSystem,$"Game is already loading...");
                return;
            }
            
            m_isLoading = true;
            m_onStartLoad?.Invoke();

            m_gameData = await m_fileDataHandler.Load<GameData>();

            if (m_gameData == null)
            {
                QRDebug.Debug(K.DebuggingChannels.SaveSystem, $"No {nameof(m_gameData)} was found. Initializing default values.");
                await NewGame();
            }
            
            Load.Current.LoadObjects();
            m_onEndLoad?.Invoke();
            m_isLoading = false;

            QRDebug.Debug(K.DebuggingChannels.SaveSystem,$"Game is load.");
        }

        [Button]
        public async Task SaveGameAsync()
        {
            if (m_isSaving)
            {
                QRDebug.DebugError(K.DebuggingChannels.SaveSystem,$"Game is already saving...");
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
            
            QRDebug.Debug(K.DebuggingChannels.SaveSystem,$"Game is save.");
        }

        [Button]
        public async Task DeleteSave()
        {
            var task = m_fileDataHandler.TryDeleteSave();
            await task;

            if (task.Result == true)
            {
                QRDebug.Debug(K.DebuggingChannels.SaveSystem, $"Save is delete.");
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

        private async void OnApplicationQuit()
        {
            await SaveGameAsync();
        }

        private void OnDestroy()
        {
            m_cancellationTokenSource.Cancel();
        }

        public static async Task<GameData> LoadInEditor()
        {
            var saveSystemSettings = SaveServiceSettings.Instance;
            var fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveSystemSettings.FullPath,
                saveSystemSettings.FullFileName);
            var gameData = await fileDataHandler.Load<GameData>();

            if (gameData == null)
            {
                QRDebug.DebugError(K.DebuggingChannels.Editor, $"There is no Game Data");
                return null;
            }
                
            var loadableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ILoadableObject>().ToArray();

            for (var i = 0; i < loadableObjects.Length; i++)
            {
                loadableObjects[i].LoadGameData(gameData as GameData);
            }
                
            QRDebug.Debug(K.DebuggingChannels.Editor, $"Load in editor is successful.");
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
            
            QRDebug.Debug(K.DebuggingChannels.Editor, $"Save in editor is successful.");
        }
        
        public static void DeleteSaveInEditor()
        {
            var saveSystemSettings = SaveServiceSettings.Instance;
            var fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler(saveSystemSettings.FullPath,
                saveSystemSettings.FullFileName);
            fileDataHandler.TryDeleteSave();
            QRDebug.Debug(K.DebuggingChannels.Editor, $"Save Data has been deleted successfully");
        }
    }
}