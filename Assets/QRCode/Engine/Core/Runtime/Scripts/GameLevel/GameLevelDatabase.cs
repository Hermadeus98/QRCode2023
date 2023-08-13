namespace QRCode.Engine.Core.GameLevel
{
    using UnityEngine;

    using System;
    
    using Sirenix.OdinInspector;

    using Toolbox;
    using Toolbox.Database;

    [CreateAssetMenu(menuName = Constants.DatabasePath.BasePath + "Game Level Database", fileName = "DB_GameLevelDatabase")]
    public class GameLevelDatabase : ScriptableObjectDatabase<GameLevelReferenceGroup>
    {
        protected override string m_databaseInformation { get => "Game Level represents a physical level in the game. Only one level can be loaded."; }
    }
    
    [Serializable][DrawWithUnity]
    public struct GameLevelReferenceGroup
    {
        [SerializeField] private GameLevel gameLevel;

        public GameLevel GameLevel => gameLevel;

        public static bool operator ==(GameLevelReferenceGroup a, GameLevelReferenceGroup b)
        {
            return a.GameLevel.name == b.GameLevel.name;
        }

        public static bool operator !=(GameLevelReferenceGroup a, GameLevelReferenceGroup b)
        {
            return a.GameLevel.name != b.GameLevel.name;
        }
    }
}
