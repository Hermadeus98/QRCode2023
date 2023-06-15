namespace QRCode.Framework
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UIAnimation
    {
        public virtual Task ShowAnimation(UIElement element, Action onStartAnimation, CancellationToken cancellationToken)
        {
            onStartAnimation?.Invoke();
            return Task.CompletedTask;
        }
        
        public virtual Task HideAnimation(UIElement element, Action onComplete, CancellationToken cancellationToken)
        {
            onComplete?.Invoke();
            return Task.CompletedTask;
        }

        public virtual Task HighLightAnimation(UIElement element, Action onHighlight, CancellationToken cancellationToken)
        {
            onHighlight?.Invoke();
            return Task.CompletedTask;
        }
    }
}
