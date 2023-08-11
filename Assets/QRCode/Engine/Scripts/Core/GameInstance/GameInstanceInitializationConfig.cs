namespace QRCode.Engine.Core
{
    using Framework.Singleton;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    /// <summary>
    /// This class contains all the initialization necessities for create the <see cref="GameInstance"/>.
    /// </summary>
    [CreateAssetMenu(menuName = "QRCode/Engine/New Game Instance Config", fileName = "New Game Instance Config")]
    public sealed class GameInstanceInitializationConfig : SingletonScriptableObject<GameInstanceInitializationConfig>
    {
        [TitleGroup("Manager Initialization")]
        [SerializeField] private AssetReference[] m_allManagersForGameInstanceInitialization = null;

        public AssetReference[] AllManagersForGameInstanceInitialization
        {
            get
            {
                return m_allManagersForGameInstanceInitialization;
            }
        }
    }
}