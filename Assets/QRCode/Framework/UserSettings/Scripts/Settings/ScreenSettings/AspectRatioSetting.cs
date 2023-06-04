namespace QRCode.Framework.Settings
{
    using System.Collections;
    using Sirenix.OdinInspector;

    public static class AspectRatioSetting
    {
        private static IEnumerable AspectRatioValues = new ValueDropdownList<string>()
        {
            { "16:9", "16:9" },
            { "16:10", "16:10" },
        };
    }
}