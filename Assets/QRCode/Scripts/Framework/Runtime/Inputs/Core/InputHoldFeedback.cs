namespace QRCode.Framework
{
    using UnityEngine;
    using UnityEngine.UI;

    public class InputHoldFeedback : MonoBehaviour
    {
        [SerializeField] private Image m_holdFeedbackImage;
        [SerializeField] private AnimationCurve m_animationCurve;

        public void Activate()
        {
            gameObject.SetActive(true);
        }
        
        public void UpdateHoldFeedback(float percent)
        {
            m_holdFeedbackImage.fillAmount = m_animationCurve.Evaluate(percent);
        }
    }
}
