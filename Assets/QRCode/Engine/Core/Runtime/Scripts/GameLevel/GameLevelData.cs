namespace QRCode.Engine.Core.GameLevels
{
    using Sirenix.OdinInspector;
    
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    /// <summary>
    /// A <see cref="GameLevelData"/> defines the tangible environment of the game, it can contains multiple scenes, contains GPE and minor logics elements.
    /// Should be a wrapper to feed in data <exception cref="GameLevel"></exception>, must be override for each new game level.
    /// </summary>
    [CreateAssetMenu(menuName = "QRCode/Engine/GameLevel/New GameLevel Data", fileName = "GameLevel_NewGameLevelData")]
    public class GameLevelData : ScriptableObject
    {
        [TitleGroup("Reference")]
        [SerializeField] private AssetReference[] m_gameLevelScenes = null;

        [SerializeField] private GameLevelModuleData[] m_gameLevelModuleData = null;
        
        public AssetReference[] GameLevelScenes => m_gameLevelScenes;

        public bool TryGetGameLevelModuleOfType<T>(out T gameLevelModuleData) where T : GameLevelModuleData
        {
            for (int i = 0; i < m_gameLevelModuleData.Length; i++)
            {
                if (m_gameLevelModuleData[i].GetType() == typeof(T))
                {
                    gameLevelModuleData = m_gameLevelModuleData[i] as T;
                    return true;
                }
            }

            gameLevelModuleData = null;
            return false;
        }
    }
}
