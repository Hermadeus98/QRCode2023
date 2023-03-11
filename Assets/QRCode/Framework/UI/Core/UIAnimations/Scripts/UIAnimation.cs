namespace QRCode.Framework
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UIAnimation
    {
        public virtual async Task ShowAnimation(UIElement element, Action onStartAnimation, CancellationToken cancellationToken)
        {
            onStartAnimation?.Invoke();
        }
        
        public virtual async Task HideAnimation(UIElement element, Action onComplete, CancellationToken cancellationToken)
        {
            onComplete?.Invoke();
        }

        public virtual async Task HighLightAnimation(UIElement element, Action onHighlight, CancellationToken cancellationToken)
        {
            onHighlight?.Invoke();
        }
    }
}
