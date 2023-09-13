namespace QRCode.Engine.Game.UI.SaveIcon
{
    using DG.Tweening;
    using QRCode.Engine.Core.SaveSystem;
    using QRCode.Engine.Core.UI;
    using QRCode.Engine.Toolbox;
    using UnityEngine;

    public class SaveIcon : UIElement
    {
        [SerializeField] private TweenParameters m_fadeTweenParameters = new TweenParameters();

        private SaveManager m_saveService = null;
        private Tween m_fadeTween = null;

        public override void Initialize()
        {
            m_saveService = SaveManager.Instance;
            
            base.Initialize();
        }

        protected override void OnEnable()
        {
            m_saveService = SaveManager.Instance;
            
            m_saveService.OnStartSave += Show;
            m_saveService.OnEndSave += Hide;
            m_saveService.OnStartLoad += Show;
            m_saveService.OnEndLoad += Hide;
            
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            m_saveService.OnStartSave -= Show;
            m_saveService.OnEndSave -= Hide;
            m_saveService.OnStartLoad -= Show;
            m_saveService.OnEndLoad -= Hide;
            
            base.OnDisable();
        }

        private void Show()
        {
            m_fadeTween?.Kill();
            m_fadeTween = CanvasGroup.DOFade(1f, m_fadeTweenParameters.Duration).SetDelay(m_fadeTweenParameters.Delay)
                .SetEase(m_fadeTweenParameters.Ease);
        }

        private void Hide()
        {
            m_fadeTween?.Kill();
            m_fadeTween = CanvasGroup.DOFade(0f, m_fadeTweenParameters.Duration).SetDelay(m_fadeTweenParameters.Delay)
                .SetEase(m_fadeTweenParameters.Ease);
        }
    }
}
