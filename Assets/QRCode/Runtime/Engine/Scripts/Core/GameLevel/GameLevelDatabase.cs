namespace QRCode.Framework.SceneManagement
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = K.DatabasePath.BasePath + "Game Level Database", fileName = "DB_GameLevelDatabase")]
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
