namespace QRCode.Utils
{
    using System;
    using UnityEngine;
    
    public static class ColorPalletsUtils
    {
        public static Color GetColorFromFlatUIPallet(FlatUIPallet flatUIPallet)
        {
            var HTLMCode = GetHTLMCode(flatUIPallet);
            if (ColorUtility.TryParseHtmlString(HTLMCode, out var outColor))
            {
                return outColor;
            }
            else
            {
                Debug.LogError($"Cannot parse {nameof(HTLMCode)} into color.");
                return Color.clear;
            }
        }

        public static string GetHTLMCode(FlatUIPallet flatUIPallet)
        {
            return flatUIPallet switch
            {
                FlatUIPallet.Turquoise => "#1abc9c",
                FlatUIPallet.Emerald => "#2ecc71",
                FlatUIPallet.PeterRiver => "#3498db",
                FlatUIPallet.Amethyst => "#9b59b6",
                FlatUIPallet.WetAsphalt => "#34495e",
                FlatUIPallet.GreenSea => "#16a085",
                FlatUIPallet.Nephritis => "#27ae60",
                FlatUIPallet.BelizeHole => "#2980b9",
                FlatUIPallet.Wisteria => "#8e44ad",
                FlatUIPallet.Midnight => "#2c3e50",
                FlatUIPallet.SunFlower => "#f1c40f",
                FlatUIPallet.Carrot => "#e67e22",
                FlatUIPallet.Alizarin => "#e74c3c",
                FlatUIPallet.Clouds => "#ecf0f1",
                FlatUIPallet.Concrete => "#95a5a6",
                FlatUIPallet.Orange => "#f39c12",
                FlatUIPallet.Pumpkin => "#d35400",
                FlatUIPallet.Pomegranate => "#c0392b",
                FlatUIPallet.Silver => "#bdc3c7",
                FlatUIPallet.Asbestos => "#7f8c8d",
                _ => throw new ArgumentOutOfRangeException(nameof(flatUIPallet), flatUIPallet, null)
            };
        }
    }

    public enum FlatUIPallet
    {
        Turquoise,
        Emerald,
        PeterRiver,
        Amethyst,
        WetAsphalt,
        GreenSea,
        Nephritis,
        BelizeHole,
        Wisteria,
        Midnight,
        SunFlower,
        Carrot,
        Alizarin,
        Clouds,
        Concrete,
        Orange,
        Pumpkin,
        Pomegranate,
        Silver,
        Asbestos,
    }
}
