namespace QRCode.Engine.Core.GameLevel
{
    using System;
    using Toolbox;
    using Sirenix.OdinInspector;
    using Toolbox.Database;
    using UnityEngine;

    [CreateAssetMenu(menuName = Constants.DatabasePath.BasePath + "Game Level Database", fileName = "DB_GameLevelDatabase")]
    public class GameLevelDatabase : ScriptableObjectDatabase<GameLevelReferenceGroup>
    {
        
    }
    
    [Serializable][DrawWithUnity]
    public struct GameLevelReferenceGroup
    {
        [SerializeField] private GameLevel gameLevel;

        public GameLevel GameLevel => gameLevel;
    }
}
