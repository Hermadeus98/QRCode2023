namespace QRCode.Framework
{
    using System;
    using Settings;
    using Settings.GameplaySettings;
    using Settings.InterfaceSettings;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Localization;

    [Serializable]
    public class UserSettingsData
    {
        [TabGroup("Screen", TextColor = "#2ecc71")] 
        [Title("General")]
        [SerializeField] [Range(0,10)]
        private int m_brightness;

        [TabGroup("Screen", TextColor = "#2ecc71")]
        [SerializeField] [Range(0, 10)]
        private int m_contrast;

        [TabGroup("Screen", TextColor = "#2ecc71")] 
        [SerializeField]
        private ColorblindModeSetting m_colorBlindModeSetting;

        [TabGroup("Screen", TextColor = "#2ecc71")]
        [SerializeField] [Range(85, 115)] [SuffixLabel("%")]
        private int m_fieldOfView;

        [TabGroup("Screen", TextColor = "#2ecc71")] 
        [SerializeField] [Range(30, 90)] [SuffixLabel("FPS")]
        private int m_fpsLimit;
        
        [TabGroup("Screen", TextColor = "#2ecc71")] 
        [Title("Display")]
        [SerializeField]
        private WindowModeSetting m_windowModeSetting;
        
        [TabGroup("Screen", TextColor = "#2ecc71")]
        [SerializeField] [ReadOnly]
        private string m_activeMonitor;
        
        [TabGroup("Screen", TextColor = "#2ecc71")] 
        [SerializeField] [ValueDropdown("@AspectRatioSetting.AspectRatioValues")]
        private string m_aspectRatio;
        
        [TabGroup("Screen", TextColor = "#2ecc71")] 
        [SerializeField] [ValueDropdown("@ResolutionSetting.ResolutionSettingValues")]
        private string m_resolution;
        
        [TabGroup("Screen", TextColor = "#2ecc71")] 
        [SerializeField] [LabelText("VSync")]
        private VSyncMode m_VSync;
        
        [TabGroup("Graphics", TextColor = "#3498db")] 
        [Title("General")]
        [SerializeField]
        private GraphicsQualitySetting m_graphicsQualitySetting;
        
        [TabGroup("Graphics", TextColor = "#3498db")] 
        [SerializeField]
        private bool m_useAdaptativeQuality;
        
        [TabGroup("Graphics", TextColor = "#3498db")] 
        [SerializeField] [ValueDropdown("@AdaptativeQualitySetting.AdaptativeQualityValues")]
        private int m_adaptativeQualitySetting;

        [TabGroup("Graphics", TextColor = "#3498db")] 
        [SerializeField] [LabelText("Anti-Aliasing Setting")]
        private AntiAliasingSetting m_antiAliasingSetting;

        [TabGroup("Graphics", TextColor = "#3498db")] 
        [Title("Post-Processing")] 
        [SerializeField]
        private DepthOfFieldSetting m_depthOfFieldSetting;
        
        [TabGroup("Graphics", TextColor = "#3498db")] 
        [SerializeField]
        private bool m_useMotionBlur;

        [TabGroup("Controls", TextColor = "#e67e22")] 
        [Title("General")]
        [SerializeField]
        private InputModePC m_inputModePC;
        
        [TabGroup("Controls", TextColor = "#e67e22")] 
        [SerializeField] [Range(10, 1000)] [SuffixLabel("ms")]
        private int m_menuHoldFactor;
        
        [TabGroup("Controls", TextColor = "#e67e22")] 
        [SerializeField] [Range(1,10)]
        private int m_mouseSensitivity;

        [TabGroup("Controls", TextColor = "#e67e22")]
        [SerializeField]
        private AxisModeSetting m_mouseXAxisModeSetting;
        
        [TabGroup("Controls", TextColor = "#e67e22")] 
        [SerializeField]
        private AxisModeSetting m_mouseYAxisModeSetting;

        [TabGroup("Controls", TextColor = "#e67e22")] 
        [SerializeField]
        private LeftHandedMouseSetting m_leftHandedMouseSetting;
        
        [TabGroup("Controls", TextColor = "#e67e22")] 
        [SerializeField]
        private bool m_lockCursor;
        
        [TabGroup("Controls", TextColor = "#e67e22")] 
        [Title("Controllers")]
        [SerializeField] [Range(1,10)]
        private int m_controllerXAxisSensitivity;
        
        [TabGroup("Controls", TextColor = "#e67e22")] 
        [SerializeField] [Range(1,10)]
        private int m_controllerYAxisSensitivity;
        
        [TabGroup("Controls", TextColor = "#e67e22")] 
        [SerializeField]
        private AxisModeSetting m_controllerXAxisModeSetting;
        
        [TabGroup("Controls", TextColor = "#e67e22")] 
        [SerializeField]
        private AxisModeSetting m_controllerYAxisModeSetting;
        
        [TabGroup("Controls", TextColor = "#e67e22")] 
        [SerializeField]
        private ControllerVibrationSetting m_controllerVibrationSetting;
        
        [TabGroup("Controls", TextColor = "#e67e22")] 
        [SerializeField]
        private SwapStickSetting m_swapStickSetting;
        
        [TabGroup("Gameplay", TextColor = "#1abc9c")] 
        [Title("General")]
        [SerializeField]
        private QuickTimeEventsInputTypeSetting m_quickTimeEventsInputTypeSetting;
        
        [TabGroup("Gameplay", TextColor = "#1abc9c")] 
        [SerializeField]
        private bool m_screenShakeSetting;
        
        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [Title("General")]
        [SerializeField] [Range(-25, 50)] private int m_interfaceAreaCalibrationSize = 12;
        
        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [SerializeField]
        private Settings.InterfaceSettings.TextSizeSetting m_textSizeSetting;
        
        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [SerializeField]
        private IconSizeSetting m_iconSizeSetting;

        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [SerializeField]
        private MenuNavigationSettings m_menuNavigationSettings;
        
        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [SerializeField][Range(1,10)] private int m_gamepadCursorSensibility = 5;
        
        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [SerializeField]
        private HoldAlternativeSetting m_holdAlternativeSetting;
        
        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [SerializeField]
        private bool m_showHUDBackground;
        
        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [SerializeField] [Range(0,100)] [SuffixLabel("%")]
        private int m_HUDBackgroundOpacity;

        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [Title("HUD: Others")]
        [SerializeField]
        private bool m_showTipsAndMessages;
        
        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [SerializeField]
        private bool m_showContextualAction;
        
        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [SerializeField]
        private bool m_showControlHints;

        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [Title("General")] 
        [SerializeField] private DB_AvailableVoiceLocalesEnum m_voiceLanguage;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [SerializeField] [Range(0, 100)] [SuffixLabel("%")]
        private int m_masterVolume;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [SerializeField] [Range(0, 100)] [SuffixLabel("%")]
        private int m_musicVolume;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [SerializeField] [Range(0, 100)] [SuffixLabel("%")]
        private int m_soundFXVolume;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [SerializeField]
        private bool m_dialogueBoost;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [Title("Subtitles & Closed Captions")]
        [SerializeField]
        private bool m_showSubtitles;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [SerializeField]
        private Settings.InterfaceSettings.TextSizeSetting m_subtitlesTextSizeSetting;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [SerializeField]
        private bool m_showSubtitleBackground;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [SerializeField] [Range(0,100)] [SuffixLabel("%")]
        private int m_subtitleBackgroundOpacity;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [SerializeField]
        private bool m_showSubtitleSpeakerName;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [SerializeField]
        private bool m_audioDescriptionTrack;

        //CONTROLS
        public int MenuHoldFactor
        {
            get => m_menuHoldFactor;
            set => m_menuHoldFactor = value;
        }
        
        //INTERFACE
        public int InterfaceAreaCalibrationSize
        {
            get => m_interfaceAreaCalibrationSize;
            set => m_interfaceAreaCalibrationSize = value;
        }
        
        public Settings.InterfaceSettings.TextSizeSetting TextSizeSetting
        {
            get => m_textSizeSetting;
            set => m_textSizeSetting = value;
        }

        public int GamepadCursorSensibility
        {
            get => m_gamepadCursorSensibility;
            set => m_gamepadCursorSensibility = value;
        }
        
        //SOUND
        public DB_AvailableVoiceLocalesEnum VoiceLanguage
        {
            get => m_voiceLanguage;
            set => m_voiceLanguage = value;
        }
        
        public bool ShowSubtitles
        {
            get => m_showSubtitles;
            set => m_showSubtitles = value;
        }

        public bool ShowSubtitleBackground
        {
            get => m_showSubtitleBackground;
            set => m_showSubtitleBackground = value;
        }

        public int SubtitleBackgroundOpacity
        {
            get => m_subtitleBackgroundOpacity;
            set => m_subtitleBackgroundOpacity = value;
        }

        public bool ShowSubtitleSpeakerName
        {
            get => m_showSubtitleSpeakerName;
            set => m_showSubtitleSpeakerName = value;
        }

        public Settings.InterfaceSettings.TextSizeSetting SubtitlesTextSizeSetting
        {
            get => m_subtitlesTextSizeSetting;
            set => m_subtitlesTextSizeSetting = value;
        }
        
        public UserSettingsData()
        {
            //SCREEN
            m_brightness = 5;
            m_contrast = 5;
            m_colorBlindModeSetting = ColorblindModeSetting.Off;
            m_fieldOfView = 100;
            m_fpsLimit = 60;
            m_activeMonitor = "NotFoundedActiveMonitor";
            m_windowModeSetting = WindowModeSetting.FullScreen;
            m_aspectRatio = "16:9";
            m_resolution = "1920 x 1080";
            m_VSync = VSyncMode.Off;

            //GRAPHICS
            m_graphicsQualitySetting = GraphicsQualitySetting.High;
            m_useAdaptativeQuality = false;
            m_adaptativeQualitySetting = 60;
            m_antiAliasingSetting = AntiAliasingSetting.High;
            m_depthOfFieldSetting = DepthOfFieldSetting.High;
            m_useMotionBlur = true;

            //CONTROLS
            m_inputModePC = InputModePC.HybridMode;
            m_menuHoldFactor = 1000;
            m_mouseSensitivity = 5;
            m_mouseXAxisModeSetting = AxisModeSetting.Normal;
            m_mouseYAxisModeSetting = AxisModeSetting.Normal;
            m_leftHandedMouseSetting = LeftHandedMouseSetting.Automatic;
            m_lockCursor = false;
            m_controllerXAxisSensitivity = 5;
            m_controllerYAxisSensitivity = 5;
            m_controllerXAxisModeSetting = AxisModeSetting.Normal;
            m_controllerYAxisModeSetting = AxisModeSetting.Normal;
            m_controllerVibrationSetting = ControllerVibrationSetting.Normal;
            m_swapStickSetting = SwapStickSetting.Off;

            //GAMEPLAY
            m_quickTimeEventsInputTypeSetting = QuickTimeEventsInputTypeSetting.Repeated;
            m_screenShakeSetting = true;

            //INTERFACE
            m_interfaceAreaCalibrationSize = 12;
            m_textSizeSetting = Settings.InterfaceSettings.TextSizeSetting.Small;
            m_iconSizeSetting = IconSizeSetting.Small;
            m_menuNavigationSettings = MenuNavigationSettings.Cursor;
            m_holdAlternativeSetting = HoldAlternativeSetting.Hold;
            m_gamepadCursorSensibility = 5;
            m_showHUDBackground = false;
            m_HUDBackgroundOpacity = 100;
            m_showTipsAndMessages = true;
            m_showContextualAction = true;
            m_showControlHints = true;

            //SOUND
            m_voiceLanguage = DB_AvailableVoiceLocalesEnum.English;
            m_masterVolume = 100;
            m_musicVolume = 100;
            m_soundFXVolume = 100;
            m_dialogueBoost = false;
            m_showSubtitles = true;
            m_subtitlesTextSizeSetting = Settings.InterfaceSettings.TextSizeSetting.Small;
            m_showSubtitleBackground = false;
            m_subtitleBackgroundOpacity = 100;
            m_showSubtitleSpeakerName = true;
            m_audioDescriptionTrack = false;
        }
    }
}