namespace QRCode.Framework
{
    using DG.Tweening;
    using UnityEngine;

    public class SaveIcon : UIElement
    {
        [SerializeField] private CanvasGroup m_canvasGroup = null;

        [SerializeField] private TweenParameters m_fadeTweenParameters = new TweenParameters();

        private ISaveService m_saveService = null;
        private Tween m_fadeTween = null;
        
        private ISaveService SaveService
        {
            get
            {
                if (m_saveService == null)
                {
                    m_saveService = ServiceLocator.Current.Get<ISaveService>();
                }

                return m_saveService;
            }
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            SaveService.OnStartSave += Show;
            SaveService.OnEndSave += Hide;
            SaveService.OnStartLoad += Show;
            SaveService.OnEndLoad += Hide;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SaveService.OnStartSave -= Show;
            SaveService.OnEndSave -= Hide;
            SaveService.OnStartLoad -= Show;
            SaveService.OnEndLoad -= Hide;
        }

        private void Show()
        {
            m_fadeTween?.Kill();
            m_fadeTween = m_canvasGroup.DOFade(1f, m_fadeTweenParameters.Duration).SetDelay(m_fadeTweenParameters.Delay)
                .SetEase(m_fadeTweenParameters.Ease);
        }

        private void Hide()
        {
            m_fadeTween?.Kill();
            m_fadeTween = m_canvasGroup.DOFade(0f, m_fadeTweenParameters.Duration).SetDelay(m_fadeTweenParameters.Delay)
                .SetEase(m_fadeTweenParameters.Ease);
        }
    }
}
