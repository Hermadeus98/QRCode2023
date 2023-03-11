namespace QRCode.Framework
{
    using System;
    using DG.Tweening;
    using Sirenix.OdinInspector;

    [Serializable]
    public struct TweenParameters<T>
    {
        [SuffixLabel("s", true)] 
        public float Delay;
        public T ToValue;
        [SuffixLabel("s", true)] 
        public float Duration;
        public Ease Ease;
    }
    
    [Serializable]
    public struct TweenParameters
    {
        [SuffixLabel("s", true)] 
        public float Delay;
        [SuffixLabel("s", true)] 
        public float Duration;
        public Ease Ease;
    }
}
