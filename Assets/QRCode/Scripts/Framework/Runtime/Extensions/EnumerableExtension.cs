namespace QRCode.Framework.Extensions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class EnumerableExtension
    {
        public static bool IsNotNullOrEmpty(this ICollection collection)
        {
            return collection != null || collection.Count != 0;
        }
        
        public static bool IsNullOrEmpty(this ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static T GetRandom<T>(this IEnumerable<T> enumerable)
        {
            var enumerable1 = enumerable as T[] ?? enumerable.ToArray();
            return enumerable1.ElementAt(Random.Range(0, enumerable1.Count()));
        }
    }
}