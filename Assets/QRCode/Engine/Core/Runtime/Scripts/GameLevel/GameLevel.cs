namespace QRCode.Engine.Core.GameLevel
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    /// <summary>
    /// A <see cref="GameLevel"/> defines the tangible environment of the game, it can contains multiple scenes, contains GPE and minor logics elements.
    /// </summary>
    [CreateAssetMenu(menuName = "QRCode/Engine/GameLevel/NewGameLevel", fileName = "GameLevel_NewGameLevel")]
    public class GameLevel : ScriptableObject
    {
        [SerializeField] private AssetReference[] m_gameLevelScenes;
        public AssetReference[] GameLevelScenes => m_gameLevelScenes;
    }
}
