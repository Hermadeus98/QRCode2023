namespace QRCode.Framework
{
    using System;
    using DG.Tweening;
    using Sirenix.OdinInspector;

    [Serializable]
    public struct TweenParameters<T>
    {
        [SuffixLabel("s")] 
        public float Delay;
        public T ToValue;
        [SuffixLabel("s")] 
        public float Duration;
        public Ease Ease;
    }
}
