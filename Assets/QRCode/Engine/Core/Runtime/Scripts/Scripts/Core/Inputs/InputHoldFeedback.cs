namespace QRCode.Engine.Core.Inputs
{
    using UI;
    using UnityEngine;
    using UnityEngine.UI;

    public class InputHoldFeedback : MonoBehaviour
    {
        [SerializeField] private Image m_holdFeedbackImage;
        
        public void Activate()
        {
            gameObject.SetActive(true);
        }
        
        public void UpdateHoldFeedback(float percent)
        {
            m_holdFeedbackImage.fillAmount = InterfaceSettings.Instance.MenuHoldFactorProgressionCurve.Evaluate(percent);
        }
    }
}
