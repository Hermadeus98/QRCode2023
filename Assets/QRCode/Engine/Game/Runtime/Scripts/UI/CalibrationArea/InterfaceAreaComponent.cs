namespace QRCode.Engine.Game.UI
{
    using System.Threading.Tasks;
    using Core.UserSettings;
    using Core.UserSettings.Events.InterfaceSettings;
    using Toolbox;
    using Sirenix.OdinInspector;
    using Toolbox.Extensions;
    using UnityEngine;
    using GameInstance = Core.GameInstance.GameInstance;

    public class InterfaceAreaComponent : MonoBehaviour
    {
        [TitleGroup(Constants.InspectorGroups.References)] [SerializeField]
        private RectTransform m_rectTransform = null;
        
        private int m_interfaceAreaCalibrationSize = 12;

        private UserSettingsData m_userSettingsData = null;
        private UserSettingsData UserSettingsData
        {
            get
            {
                if (m_userSettingsData == null)
                {
                    m_userSettingsData = UserSettingsManager.Instance.GetUserSettingsData();
                }

                return m_userSettingsData;
            }
        }
        
        private async void OnEnable()
        {
            m_rectTransform ??= GetComponent<RectTransform>();
            
            InterfaceAreaCalibrationEvent.Register(AdjustInterfaceAreaCalibrationFromSettings);

            while (GameInstance.Instance.IsReady == false)
            {
                await Task.Yield();
            }
            
            if (UserSettingsManager.Instance.IsInit)
            {
                AdjustInterfaceAreaCalibrationFromSettings(UserSettingsData.InterfaceAreaCalibrationSize);
            }
        }

        private void OnDisable()
        {
            InterfaceAreaCalibrationEvent.Unregister(AdjustInterfaceAreaCalibrationFromSettings);
        }

        private void AdjustInterfaceAreaCalibrationFromSettings(int interfaceAreaCalibrationSize)
        {
            m_rectTransform.SetLeft(interfaceAreaCalibrationSize);
            m_rectTransform.SetRight(interfaceAreaCalibrationSize);
            m_rectTransform.SetBottom(interfaceAreaCalibrationSize);
            m_rectTransform.SetTop(interfaceAreaCalibrationSize);
        }
    }
}
