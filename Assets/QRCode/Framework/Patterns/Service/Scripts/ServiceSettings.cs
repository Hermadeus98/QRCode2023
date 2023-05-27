namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = K.SettingsPath.ServiceSettingsPath, fileName = "STG_ServiceSettings")]
    public class ServiceSettings : Settings<ServiceSettings>
    {
        [TitleGroup("Framework Service")] [SerializeField]
        private AssetReference m_sceneManagementService = null;

        [TitleGroup("Input Service")] [SerializeField]
        private AssetReference m_inputManagementService = null;

        public AssetReference SceneManagementService => m_sceneManagementService;
        public AssetReference InputManagementService => m_inputManagementService;
    }
}
