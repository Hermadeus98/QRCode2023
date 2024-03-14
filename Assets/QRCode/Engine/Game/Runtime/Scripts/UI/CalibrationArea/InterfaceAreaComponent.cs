namespace QRCode.Engine.Game.UI
{
    using QRCode.Engine.Core.UserSettings.Events.InterfaceSettings;
    using QRCode.Engine.Toolbox.Extensions;
    using UnityEngine;

    /// <summary>
    /// The component manage interface calibration from the user settings data.
    /// </summary>
    public class InterfaceAreaComponent : MonoBehaviour
    {
        #region Fields
        private RectTransform _rectTransform = null;
        #endregion Fields

        #region Properties
        private RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }
        #endregion Properties
        
        private void OnEnable()
        {
            InterfaceAreaCalibrationEvent.Register(AdjustInterfaceAreaCalibrationFromSettings);
        }

        private void OnDisable()
        {
            InterfaceAreaCalibrationEvent.Unregister(AdjustInterfaceAreaCalibrationFromSettings);
        }

        private void AdjustInterfaceAreaCalibrationFromSettings(int interfaceAreaCalibrationSize)
        {
            RectTransform.SetLeft(interfaceAreaCalibrationSize);
            RectTransform.SetRight(interfaceAreaCalibrationSize);
            RectTransform.SetBottom(interfaceAreaCalibrationSize);
            RectTransform.SetTop(interfaceAreaCalibrationSize);
        }
    }
}
