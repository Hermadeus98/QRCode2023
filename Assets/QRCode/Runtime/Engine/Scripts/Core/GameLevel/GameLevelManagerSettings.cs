namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Localization;

    [CreateAssetMenu(menuName = K.SettingsPath.GameLevelManagerSettingsPath, fileName = "STG_GameLevelManagerSettings")]
    public class GameLevelManagerSettings : Settings<GameLevelManagerSettings>
    {
        [TitleGroup(K.InspectorGroups.Settings)]
        [SerializeField] private float m_minimalLoadDurationBefore = .5f;
        
        [TitleGroup(K.InspectorGroups.Settings)]
        [SerializeField] private float m_minimalLoadDurationAfter = .5f;

        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private LocalizedString m_loadingLocalizedString = null;

        public float MinimalLoadDurationBefore => m_minimalLoadDurationBefore;
        public float MinimalLoadDurationAfter => m_minimalLoadDurationAfter;

        public string LoadingLocalizedString
        {
            get
            {
                if (m_loadingLocalizedString == null)
                {
                    return "NULL TXT";
                }
                else
                {
                    return m_loadingLocalizedString.GetLocalizedString();
                }
            }
        }
    }
}