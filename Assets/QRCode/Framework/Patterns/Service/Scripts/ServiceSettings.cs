namespace QRCode.Framework
{
    using Singleton;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = K.SettingsPath.ServiceSettingsPath, fileName = "STG_ServiceSettings")]
    public class ServiceSettings : SingletonScriptableObject<ServiceSettings>
    {
        [TitleGroup("Framework Service")] [SerializeField]
        private ISceneManagementService m_sceneManagementService = null;

        public ISceneManagementService SceneManagementService => m_sceneManagementService;
    }
}
