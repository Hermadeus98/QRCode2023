namespace QRCode.Engine.Core.GameLevels
{
    using Toolbox;
    using Sirenix.OdinInspector;
    using Toolbox.Settings;
    using UnityEngine;
    using UnityEngine.Localization;

    [CreateAssetMenu(menuName = Constants.SettingsPath.GameLevelManagerSettingsPath, fileName = "STG_GameLevelManagerSettings")]
    public class GameLevelManagerSettings : Settings<GameLevelManagerSettings>
    {
        #region Fields
        #region Serialized
        [TitleGroup(Constants.InspectorGroups.Settings)] 
        [Tooltip("Default progress description display on the loading screen when a GameLevel is loading.")]
        [SerializeField] private LocalizedString _loadingLocalizedString = null;
        #endregion Serialized
        #endregion Fields

        #region Properties
        public string LoadingLocalizedString
        {
            get
            {
                if (_loadingLocalizedString == null)
                {
                    return "NULL TXT";
                }
                
                return _loadingLocalizedString.GetLocalizedString();
            }
        }
        #endregion Properties
    }
}
