namespace QRCode.Engine.Toolbox.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class MathExtensions
    {
        public static float GetAverage(this ICollection<float> floats)
        {
            var total = floats.GetTotal();
            return total / floats.Count;
        }

        public static float GetTotal(this ICollection<float> floats)
        {
            var total = 0f;
            for (int i = 0; i < floats.Count; i++)
            {
                total += floats.ElementAt(i);
            }

            return total;
        }
    }
}
