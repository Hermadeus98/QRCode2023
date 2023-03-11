namespace QRCode.Framework
{
    using DG.Tweening;
    using UnityEngine;

    public static class RectTransformExtensions
    {
        public static void SetLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }
 
        public static void SetRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }
 
        public static void SetTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }
 
        public static void SetBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }

        public static Tween DoLeft(this RectTransform rt, float value, float duration)
        {
            return DOTween.To(
                () => rt.offsetMin,
                (x) => rt.offsetMin = x,
                new Vector2(value, rt.offsetMin.y),
                duration
            );
        }
        
        public static Tween DoRight(this RectTransform rt, float value, float duration)
        {
            return DOTween.To(
                () => rt.offsetMax,
                (x) => rt.offsetMax = x,
                new Vector2(-value, rt.offsetMax.y),
                duration
            );
        }
        
        public static Tween DoTop(this RectTransform rt, float value, float duration)
        {
            return DOTween.To(
                () => rt.offsetMax,
                (x) => rt.offsetMax = x,
                new Vector2(-value, rt.offsetMax.x),
                duration
            );
        }
        
        public static Tween DoBottom(this RectTransform rt, float value, float duration)
        {
            return DOTween.To(
                () => rt.offsetMin,
                (x) => rt.offsetMin = x,
                new Vector2(value, rt.offsetMin.x),
                duration
            );
        }
    }
}
