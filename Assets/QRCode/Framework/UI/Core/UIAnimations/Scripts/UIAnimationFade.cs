namespace QRCode.Framework
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public class UIAnimationFade : UIAnimation
    {
        [TitleGroup(K.InspectorGroups.Settings)]
        [SuffixLabel("@m_showTweenParameters.Duration")][SuffixLabel("Duration = ")]
        [SuffixLabel("@m_showTweenParameters.Ease.ToString()")][SuffixLabel("Ease = ")]
        [SerializeField] private TweenParameters<float> m_showTweenParameters;
        
        [TitleGroup(K.InspectorGroups.Settings)]
        [SuffixLabel("@m_hideTweenParameters.Duration")][SuffixLabel("Duration = ")]
        [SuffixLabel("@m_hideTweenParameters.Ease.ToString()")][SuffixLabel("Ease = ")]
        [SerializeField] private TweenParameters<float> m_hideTweenParameters;

        public override async Task ShowAnimation(UIElement element, Action onStartAnimation, CancellationToken cancellationToken)
        {
            onStartAnimation?.Invoke();
            element.CanvasGroup.DOFade(m_showTweenParameters.ToValue, m_showTweenParameters.Duration).SetEase(m_showTweenParameters.Ease).SetDelay(m_showTweenParameters.Delay);
            await Task.Delay(TimeSpan.FromSeconds(m_showTweenParameters.Duration), cancellationToken);
        }

        public override async Task HideAnimation(UIElement element, Action onComplete, CancellationToken cancellationToken)
        {
            element.CanvasGroup.DOFade(m_hideTweenParameters.ToValue, m_hideTweenParameters.Duration).SetEase(m_hideTweenParameters.Ease).SetDelay(m_hideTweenParameters.Delay);
            await Task.Delay(TimeSpan.FromSeconds(m_hideTweenParameters.Duration), cancellationToken);
            onComplete?.Invoke();
        }
    }
}
