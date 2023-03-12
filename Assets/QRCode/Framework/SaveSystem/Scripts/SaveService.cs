﻿namespace QRCode.Framework
{
    using System;
    using System.Threading.Tasks;
    using Debugging;
    using Sirenix.OdinInspector;

    //https://www.google.com/search?client=firefox-b-d&q=save+system+unity#fpstate=ive&vld=cid:47854c46,vid:aUi9aijvpgs,st:899
    public class SaveService : SerializedMonoBehaviour , ISaveService
    {
        private GameData m_gameData = null;
        private IFileDataHandler m_fileDataHandler = null;

        private bool m_isSaving = false;
        private bool m_isLoading = false;

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
            m_fileDataHandler = FileDataHandlerFactory.CreateFileDataHandler();
            await LoadGame();
        }

        public Task NewGame()
        {
            m_gameData = new GameData();
            QRDebug.Debug(K.DebuggingChannels.SaveSystem, $"New game data was created...");
            return Task.CompletedTask;
        }

        public async Task LoadGame()
        {
            if (m_isLoading)
            {
                QRDebug.DebugError(K.DebuggingChannels.SaveSystem,$"Game is already loading...");
                return;
            }
            
            m_isLoading = true;
            m_onStartLoad?.Invoke();
            m_gameData = await m_fileDataHandler.Load();
            m_onEndLoad?.Invoke();
            m_isLoading = false;

            if (m_gameData == null)
            {
                QRDebug.Debug(K.DebuggingChannels.SaveSystem, $"No {nameof(m_gameData)} was found. Initializing default values.");
                await NewGame();
            }
            
            QRDebug.Debug(K.DebuggingChannels.SaveSystem,$"Game is load.");
        }

        public async Task SaveGame()
        {
            if (m_isSaving)
            {
                QRDebug.DebugError(K.DebuggingChannels.SaveSystem,$"Game is already saving...");
                return;
            }   
            
            m_isSaving = true;
            m_onStartSave?.Invoke();
            await m_fileDataHandler.Save(m_gameData);
            m_onEndSave?.Invoke();
            m_isSaving = false;
            
            QRDebug.Debug(K.DebuggingChannels.SaveSystem,$"Game is save.");
        }

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

        private async void OnApplicationQuit()
        {
            await SaveGame();
        }
    }
}