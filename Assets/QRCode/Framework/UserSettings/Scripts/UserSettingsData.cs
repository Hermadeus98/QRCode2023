namespace QRCode.Framework
{
    using System;
    using Settings;
    using Settings.GameplaySettings;
    using Settings.InterfaceSettings;
    using Sirenix.OdinInspector;
    using UnityEngine;

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
        private float m_menuHoldFactor;
        
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
        [SerializeField]
        private TextSizeSetting m_textSizeSetting;
        
        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [SerializeField]
        private IconSizeSetting m_iconSizeSetting;

        [TabGroup("Interface", TextColor = "#f1c40f")] 
        [SerializeField]
        private MenuNavigationSettings m_menuNavigationSettings;
        
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
        private SubtitlesTextSizeSetting m_subtitlesTextSizeSetting;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [SerializeField]
        private bool m_showSubtitleBackground;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [SerializeField] [Range(0,100)] [SuffixLabel("%")]
        private int m_subtitleBackgroundOpacity;
        
        [TabGroup("Sound", TextColor = "#9b59b6")] 
        [SerializeField]
        private bool m_showSubtitleSpeakerName;
        
        public UserSettingsData()
        {
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

            m_graphicsQualitySetting = GraphicsQualitySetting.High;
            m_useAdaptativeQuality = false;
            m_adaptativeQualitySetting = 60;
            m_antiAliasingSetting = AntiAliasingSetting.High;
            m_depthOfFieldSetting = DepthOfFieldSetting.High;
            m_useMotionBlur = true;

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

            m_quickTimeEventsInputTypeSetting = QuickTimeEventsInputTypeSetting.Repeated;
            m_screenShakeSetting = true;

            m_textSizeSetting = TextSizeSetting.Small;
            m_iconSizeSetting = IconSizeSetting.Small;
            m_menuNavigationSettings = MenuNavigationSettings.Cursor;
            m_holdAlternativeSetting = HoldAlternativeSetting.Hold;
            m_showHUDBackground = false;
            m_HUDBackgroundOpacity = 100;
            m_showTipsAndMessages = true;
            m_showContextualAction = true;
            m_showControlHints = true;

            m_masterVolume = 100;
            m_musicVolume = 100;
            m_soundFXVolume = 100;
            m_dialogueBoost = false;
            m_showSubtitles = true;
            m_subtitlesTextSizeSetting = SubtitlesTextSizeSetting.Small;
            m_showSubtitleBackground = false;
            m_subtitleBackgroundOpacity = 100;
            m_showSubtitleSpeakerName = true;
        }

        public UserSettingsData(UserSettingsService userSettingsService)
        {
            var defaultUserSettingsData = userSettingsService.GetUserSettingsData();
            m_brightness = defaultUserSettingsData.m_brightness;
            m_contrast = defaultUserSettingsData.m_contrast;
            m_colorBlindModeSetting = defaultUserSettingsData.m_colorBlindModeSetting;
            m_fieldOfView = defaultUserSettingsData.m_fieldOfView;
            m_fpsLimit = defaultUserSettingsData.m_fpsLimit;
            m_activeMonitor = defaultUserSettingsData.m_activeMonitor;
            m_windowModeSetting = defaultUserSettingsData.m_windowModeSetting;
            m_aspectRatio = defaultUserSettingsData.m_aspectRatio;
            m_resolution = defaultUserSettingsData.m_resolution;
            m_VSync = defaultUserSettingsData.m_VSync;

            m_graphicsQualitySetting = defaultUserSettingsData.m_graphicsQualitySetting;
            m_useAdaptativeQuality = defaultUserSettingsData.m_useAdaptativeQuality;
            m_adaptativeQualitySetting = defaultUserSettingsData.m_adaptativeQualitySetting;
            m_antiAliasingSetting = defaultUserSettingsData.m_antiAliasingSetting;
            m_depthOfFieldSetting = defaultUserSettingsData.m_depthOfFieldSetting;
            m_useMotionBlur = defaultUserSettingsData.m_useMotionBlur;
            
            m_inputModePC = defaultUserSettingsData.m_inputModePC;
            m_menuHoldFactor = defaultUserSettingsData.m_menuHoldFactor;
            m_mouseSensitivity = defaultUserSettingsData.m_mouseSensitivity;
            m_mouseXAxisModeSetting = defaultUserSettingsData.m_mouseXAxisModeSetting;
            m_mouseYAxisModeSetting = defaultUserSettingsData.m_mouseYAxisModeSetting;
            m_leftHandedMouseSetting = defaultUserSettingsData.m_leftHandedMouseSetting;
            m_lockCursor = defaultUserSettingsData.m_lockCursor;
            m_controllerXAxisSensitivity = defaultUserSettingsData.m_controllerXAxisSensitivity;
            m_controllerYAxisSensitivity = defaultUserSettingsData.m_controllerYAxisSensitivity;
            m_controllerXAxisModeSetting = defaultUserSettingsData.m_controllerXAxisModeSetting;
            m_controllerYAxisModeSetting = defaultUserSettingsData.m_controllerYAxisModeSetting;
            m_controllerVibrationSetting = defaultUserSettingsData.m_controllerVibrationSetting;
            m_swapStickSetting = defaultUserSettingsData.m_swapStickSetting;

            m_quickTimeEventsInputTypeSetting = defaultUserSettingsData.m_quickTimeEventsInputTypeSetting;
            m_screenShakeSetting = defaultUserSettingsData.m_screenShakeSetting;

            m_textSizeSetting = defaultUserSettingsData.m_textSizeSetting;
            m_iconSizeSetting = defaultUserSettingsData.m_iconSizeSetting;
            m_menuNavigationSettings = defaultUserSettingsData.m_menuNavigationSettings;
            m_holdAlternativeSetting = defaultUserSettingsData.m_holdAlternativeSetting;
            m_showHUDBackground = defaultUserSettingsData.m_showHUDBackground;
            m_HUDBackgroundOpacity = defaultUserSettingsData.m_HUDBackgroundOpacity;
            m_showTipsAndMessages = defaultUserSettingsData.m_showTipsAndMessages;
            m_showContextualAction = defaultUserSettingsData.m_showContextualAction;
            m_showControlHints = defaultUserSettingsData.m_showControlHints;

            m_masterVolume = defaultUserSettingsData.m_masterVolume;
            m_musicVolume = defaultUserSettingsData.m_musicVolume;
            m_soundFXVolume = defaultUserSettingsData.m_soundFXVolume;
            m_dialogueBoost = defaultUserSettingsData.m_dialogueBoost;
            m_showSubtitles = defaultUserSettingsData.m_showSubtitles;
            m_subtitlesTextSizeSetting = defaultUserSettingsData.m_subtitlesTextSizeSetting;
            m_showSubtitleBackground = defaultUserSettingsData.m_showSubtitleBackground;
            m_subtitleBackgroundOpacity = defaultUserSettingsData.m_subtitleBackgroundOpacity;
            m_showSubtitleSpeakerName = defaultUserSettingsData.m_showSubtitleSpeakerName;
        }
    }
}