namespace QRCode.Framework
{
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.UI;

    public class InputIcon : InputIconBase
    {
        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private bool m_preserveAspect = true;
        
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] private Image m_icon;


        private Sequence m_onPerformInputSequence;

        protected override void OnEnable()
        {
            if (m_feedbackOnPerformInput)
            {
                m_icon.DOFade(.8f, 0f);
            }
            
            base.OnEnable();
        }

        protected override void LoadIcon()
        {
            base.LoadIcon();

            var sanitizeControlScheme = m_currentControlScheme.Replace(" ", "");
            InputMapDatabase.TryGetInDatabase(sanitizeControlScheme, out var inputMap);
            var icon = inputMap.FindIcon(m_currentDisplayName, m_alternativeIconIndex);

            m_icon.sprite = icon;

            gameObject.name = $"Input Icon [{m_currentDisplayName}]";
            m_icon.preserveAspect = m_preserveAspect;
        }

        protected override void OnPerformInput(InputAction.CallbackContext context)
        {
            base.OnPerformInput(context);
            
            if (m_feedbackOnPerformInput)
            {
                m_onPerformInputSequence?.Kill();
                m_onPerformInputSequence = DOTween.Sequence();
                m_onPerformInputSequence.Append(m_icon.DOFade(1f, .2f));
                m_onPerformInputSequence.Append(m_icon.DOFade(.8f, .2f).SetDelay(.2f));
                m_onPerformInputSequence.Play();
            }
        }
    }
}
