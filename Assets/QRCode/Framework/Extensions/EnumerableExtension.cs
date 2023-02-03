namespace QRCode.Framework.Extensions
{
    using System.Collections;

    public static class EnumerableExtension
    {
        public static bool IsNotNullOrEmpty(this ICollection enumerable)
        {
            return enumerable != null || enumerable.Count != 0;
        }
    }
}
