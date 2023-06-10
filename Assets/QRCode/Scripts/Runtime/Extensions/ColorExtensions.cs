namespace QRCode.Framework.Extensions
{
    using UnityEngine;

    public static class ColorExtensions
    {
        public static Color FromHex(this Color color, string htmlString)
        {
            if(ColorUtility.TryParseHtmlString(htmlString, out var outColor))
            {
                return outColor;
            }
            else
            {
                Debug.LogError($"Cannot Parse HtlmString into color.");
                return Color.clear;
            }
        }

        public static string ToHex(this Color color)
        {
            return "#" + ColorUtility.ToHtmlStringRGBA(color);
        }
    }
}
