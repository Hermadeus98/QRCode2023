namespace QRCode.Engine.Core
{
    using Managers;
    using Sirenix.OdinInspector;
    using UnityEngine;

    /// <summary>
    /// This class contains all the initialization necessities for create the <see cref="GameInstance"/>.
    /// </summary>
    public sealed class GameInstanceInitializationDataComponent : SerializedMonoBehaviour
    {
        [TitleGroup("Manager Initialization")]
        [SerializeField] private IManager[] m_allManagersForGameInstanceInitialization = null;

        public IManager[] AllManagersForGameInstanceInitialization
        {
            get
            {
                return m_allManagersForGameInstanceInitialization;
            }
        }
    }
}