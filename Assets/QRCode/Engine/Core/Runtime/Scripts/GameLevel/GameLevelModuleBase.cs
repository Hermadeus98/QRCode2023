﻿namespace QRCode.Engine.Core.GameLevels
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A game level module contains behaviour of the level, this is not micro behaviors like GPE, but macro behaviors like a chunk system or a checkpoint system.
    /// </summary>
    public abstract class GameLevelModuleBase<T> : IGameLevelModule where T : GameLevelModuleData
    {
        protected T m_moduleData = null;
        
        public GameLevelLoadingInfo GameLevelLoadingInfo
        {
            get => m_moduleData.GameLevelLoadingInfo; 
            set => m_moduleData.GameLevelLoadingInfo = value;
        }

        public GameLevelModuleBase(T moduleData)
        {
            m_moduleData = moduleData;
        }

        public abstract Task Load(CancellationToken cancellationToken, Action onLoading, IProgress<float> progress);

        public abstract void Delete();
    }
}