namespace QRCode.Framework.Settings
{
    using System.Collections;
    using Sirenix.OdinInspector;

    public class ResolutionSetting
    {
        private static IEnumerable ResolutionSettingValues = new ValueDropdownList<string>()
        {
            { "1208 x 720", "1208 x 720" },
            { "1360 x 768", "1360 x 768" },
            { "1366 x 768", "1366 x 768" },
            { "1600 x 900", "1600 x 900" },
            { "1768 x 992", "1768 x 992" },
            { "1920 x 1080", "1920 x 1080" },
            { "2560 x 1440", "2560 x 1440" },
        };
    }
}