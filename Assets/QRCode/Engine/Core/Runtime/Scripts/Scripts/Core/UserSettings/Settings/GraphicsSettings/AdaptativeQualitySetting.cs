namespace QRCode.Engine.Core.UserSettings.Settings.GraphicsSettings
{
    using System.Collections;
    using Sirenix.OdinInspector;

    public class AdaptativeQualitySetting
    {
        private static IEnumerable AdaptativeQualityValues = new ValueDropdownList<int>()
        {
            { "30 FPS", 30 },
            { "45 FPS", 45 },
            { "60 FPS", 60 },
        };
    }
}