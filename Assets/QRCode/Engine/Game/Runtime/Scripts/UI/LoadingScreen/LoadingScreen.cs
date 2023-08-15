namespace QRCode.Engine.Game.UI.LoadingScreen
{
    using UnityEngine;
    
    using System.Threading.Tasks;
    
    using Sirenix.OdinInspector;
    using TMPro;
    
    using Core.GameLevels;
    using Core.UI;
    using Core.UI.LoadingScreen;
    using Debugging;
    using ProgressBar;
    using Constants = Toolbox.Constants;

    public class LoadingScreen : UIView, ILoadingScreen
    {
        [ToggleGroup("m_useProgressBar","Use Progress Bar")] 
        [SerializeField] protected bool m_useProgressBar = false;

        [ToggleGroup("m_useProgressBar")] 
        [SerializeField] protected ProgressBar m_progressionSlider = null;

        [ToggleGroup("m_useProgressionDescription", "Use Progression Description")]
        [SerializeField] protected bool m_useProgressionDescription = false;

        [ToggleGroup("m_useProgressionDescription")] 
        [SerializeField] protected TextMeshProUGUI m_progressionDescriptionText = null;

        [ToggleGroup("m_useTooltips", "Use Tooltips")] 
        [SerializeField] protected bool m_useTooltips = false;

        [ToggleGroup("m_useTooltips")] 
        [SerializeField] protected TextMeshProUGUI m_tooltipsText = null;

        public override void Initialize()
        {
            base.Initialize();
            
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

        public override async Task Show()
        {
            QRDebug.Debug(Constants.DebuggingChannels.Game, $"Loading Screen show.");
            
            await base.Show();
            
            if (m_useProgressBar)
            {
                m_progressionSlider.UpdateProgressBar(0f);
            }
        }

        public override async Task Hide()
        {
            await base.Hide();
            QRDebug.Debug(Constants.DebuggingChannels.Game, $"Loading Screen hide.");
        }

        public virtual void Progress(SceneLoadingInfo loadingInfo)
        {
            if (m_useProgressionDescription)
            {
                m_progressionDescriptionText.SetText(loadingInfo.ProgressDescription);
            }
            
            if (m_useProgressBar)
            {
                m_progressionSlider.UpdateProgressBar(loadingInfo.GlobalProgress);
            }
        }
    }
}
