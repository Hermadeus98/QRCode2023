﻿namespace QRCode.Engine.Core.GameInstance
{
    using QRCode.Engine.Core.Boot;
    using UnityEngine;
    
    using Toolbox.Pattern.Singleton;

    /// <summary>
    /// This class contains all the initialization necessities for create the <see cref="GameInstance"/>.
    /// </summary>
    [CreateAssetMenu(menuName = "QRCode/Engine/New Game Instance Config", fileName = "New Game Instance Config")]
    public sealed class GameInstanceInitializationConfig : SingletonScriptableObject<GameInstanceInitializationConfig>
    {
        [SerializeField] private Boot _boot = null;
    }
}