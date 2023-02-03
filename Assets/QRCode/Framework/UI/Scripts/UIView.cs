namespace QRCode.Framework
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class UIView : SerializedMonoBehaviour
    {
        [SerializeField] private TweenParameters<float> m_showTweenParameters;
        [SerializeField] private TweenParameters<float> m_hideTweenParameters;

        private CancellationTokenSource m_cancellationTokenSource = new CancellationTokenSource();
        
        public async Task Show()
        {
            GetComponent<CanvasGroup>().DOFade(m_showTweenParameters.ToValue, m_showTweenParameters.Duration).SetEase(m_showTweenParameters.Ease).SetDelay(m_showTweenParameters.Delay);
            await Task.Delay(TimeSpan.FromSeconds(m_showTweenParameters.Duration), m_cancellationTokenSource.Token);
        }

        public async Task Hide()
        {
            GetComponent<CanvasGroup>().DOFade(m_hideTweenParameters.ToValue, m_hideTweenParameters.Duration).SetEase(m_hideTweenParameters.Ease).SetDelay(m_hideTweenParameters.Delay);
            await Task.Delay(TimeSpan.FromSeconds(m_hideTweenParameters.Duration), m_cancellationTokenSource.Token);
        }

        private void OnDestroy()
        {
            m_cancellationTokenSource.Cancel();
        }
    }
}
