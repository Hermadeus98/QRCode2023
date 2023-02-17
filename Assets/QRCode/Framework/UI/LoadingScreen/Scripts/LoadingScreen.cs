namespace QRCode.Framework
{
    using SceneManagement;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class LoadingScreen : UIView, ILoadingScreen
    {
        [ToggleGroup("m_useProgressBar","Use Progress Bar")] 
        [SerializeField] protected bool m_useProgressBar = false;

        [ToggleGroup("m_useProgressBar")] 
        [SerializeField] protected Slider m_progressionSlider = null;

        [ToggleGroup("m_useProgressionDescription", "Use Progression Description")]
        [SerializeField] protected bool m_useProgressionDescription = false;

        [ToggleGroup("m_useProgressionDescription")] 
        [SerializeField] protected TextMeshProUGUI m_progressionDescriptionText = null;

        [ToggleGroup("m_useTooltips", "Use Tooltips")] 
        [SerializeField] protected bool m_useTooltips = false;

        [ToggleGroup("m_useTooltips")] 
        [SerializeField] protected TextMeshProUGUI m_tooltipsText = null;

        protected override void Start()
        {
            base.Start();

            if (!m_useTooltips)
            {
                m_tooltipsText?.gameObject.SetActive(false);
            }

            if (!m_useProgressBar)
            {
                m_progressionSlider?.gameObject.SetActive(false);
            }

            if (!m_useProgressionDescription)
            {
                m_progressionDescriptionText?.gameObject.SetActive(false);
            }
        }

        public virtual void Progress(SceneLoadingInfo loadingInfo)
        {
            if (m_useProgressionDescription)
            {
                m_progressionDescriptionText.SetText(loadingInfo.ProgressDescription);
            }
            
            if (m_useProgressBar)
            {
                m_progressionSlider.value = loadingInfo.GlobalProgress;
            }
        }
    }
}
