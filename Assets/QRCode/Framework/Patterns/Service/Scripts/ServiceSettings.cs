namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = K.SettingsPath.ServiceSettingsPath, fileName = "STG_ServiceSettings")]
    public class ServiceSettings : Settings<ServiceSettings>
    {
        [TitleGroup("Framework Service")] [SerializeField]
        private AssetReference m_levelLoadingManagementService = null;

        [TitleGroup("Input Service")] [SerializeField]
        private AssetReference m_inputManagementService = null;

        public AssetReference LevelLoadingManagementService => m_levelLoadingManagementService;
        public AssetReference InputManagementService => m_inputManagementService;
    }
}
