namespace QRCode.Engine.Core.GameLevels
{
    using UnityEngine;

    using System;
    
    using Sirenix.OdinInspector;

    using Toolbox;
    using Toolbox.Database;

    [CreateAssetMenu(menuName = Constants.DatabasePath.BasePath + "Game Level Database", fileName = "DB_GameLevelDatabase")]
    public class GameLevelDatabase : ScriptableObjectDatabase<GameLevelData>
    {
        protected override string m_databaseInformation { get => "Game Level represents a physical level in the game. Only one level can be loaded."; }
    }
}
