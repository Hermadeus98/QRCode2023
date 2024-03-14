namespace QRCode.Engine.Core.UI
{
    using System.Threading;
    using System.Threading.Tasks;
    using Animations;
    using Toolbox;
    using Sirenix.OdinInspector;
    using UnityEngine;

    /// <summary>
    /// UIView contains UI Element and refresh them.
    /// </summary>
    public class UIView : UIElement
    {
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [SerializeField] private string m_viewName = "";
        [TitleGroup(Constants.InspectorGroups.Settings)] [SerializeField]
        private bool m_startHide = false;

        [TitleGroup(Constants.InspectorGroups.Settings)] [SerializeField]
        private UIAnimationFade m_animationFade = new UIAnimationFade();

        private CancellationTokenSource m_cancellationTokenSource = new CancellationTokenSource();

        public string ViewName { get { return m_viewName; } }
        
        public override void Initialize()
        {
            m_cancellationTokenSource = new CancellationTokenSource();
            
            base.Initialize();

            if (m_startHide)
            {
                gameObject.SetActive(false);
            }
        }

        public virtual async Task Show()
        {
            gameObject.SetActive(true);
            RefreshUI();
            OnShow();
            await m_animationFade.ShowAnimation(this, null, m_cancellationTokenSource.Token);
        }

        public virtual async Task Hide()
        {
            await m_animationFade.HideAnimation(this, null, m_cancellationTokenSource.Token);
            gameObject.SetActive(false);
        }

        public override void Delete()
        {
            if (m_cancellationTokenSource != null)
            {
                m_cancellationTokenSource.Cancel();
                m_cancellationTokenSource.Dispose();
                m_cancellationTokenSource = null;
            }

            base.Delete();
        }
    }
}