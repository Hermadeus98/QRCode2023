namespace QRCode.Engine.Core.UI.Cursor
{
    using System.Collections;
    using System.Threading.Tasks;
    using Toolbox;
    using Inputs;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.LowLevel;
    using UnityEngine.InputSystem.UI;
    using UnityEngine.InputSystem.Users;
    using UserSettings;
    using UserSettings.Events.InterfaceSettings;
    using UserSettings.Settings.InterfaceSettings;
    using GameInstance = GameInstance;

    public class GamepadCursor : UIElement
    {
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [SerializeField] private bool m_isMain = false;

        [TitleGroup(Constants.InspectorGroups.References)] 
        [SerializeField] private VirtualMouseInput m_virtualMouse;
        
        private static GamepadCursor Main;
        private IInputManagementService m_inputManagementService = null;

        private MenuNavigationSettings m_menuNavigationSettings;
        private float m_initialSpeed;
        private bool m_isActive = true;

        public bool IsActive
        {
            get => m_isActive;
            set
            {
                if (value)
                {
                    Activate();
                }
                else
                {
                    Deactivate();
                }
                m_isActive = value;
            }
        }
        
        private UserSettingsData m_userSettingsData = null;
        private UserSettingsData UserSettingsData
        {
            get
            {
                if (m_userSettingsData == null)
                {
                    m_userSettingsData = UserSettingsManager.Instance.GetUserSettingsData();
                }

                return m_userSettingsData;
            }
        }

        protected override void Start()
        {
            if (m_isMain)
            {
                Main = this;
            }

            m_inputManagementService = InputManager.Instance;

            base.Start();
            m_initialSpeed = m_virtualMouse.cursorSpeed;
        }

        private void Update()
        {
            ClampPosition();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            GamepadCursorSensibilityEvent.Register(UpdateSensibilityFromSettings);
            MenuNavigationSettingEvent.Register(UpdateMenuNavigationModeFromSettings);
            Init();
        }

        private async void Init()
        {
            while (GameInstance.GameInstance.Instance.IsReady == false)
            {
                await Task.Yield();
            }
            
            UpdateSensibilityFromSettings(UserSettingsData.GamepadCursorSensibility);
            InputUser.onChange += OnControlsChanged;
            
            SetActivationInFunctionOfScheme();
            ShouldBeDeactivated();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GamepadCursorSensibilityEvent.Unregister(UpdateSensibilityFromSettings);
            MenuNavigationSettingEvent.Unregister(UpdateMenuNavigationModeFromSettings);
            InputUser.onChange -= OnControlsChanged;
        }

        private void Activate()
        {
            if (ShouldBeDeactivated())
            {
                return;
            }
            
            CanvasGroup.alpha = 1f;
        }

        private void Deactivate()
        {
            CanvasGroup.alpha = 0f;
        }
        
        private void OnControlsChanged(InputUser inputUser, InputUserChange inputUserChange, InputDevice inputDevice)
        {
            if (IsActive == false)
            {
                Deactivate();
                return;
            }
            
            if (inputUserChange == InputUserChange.ControlsChanged)
            {
                SetActivationInFunctionOfScheme();
            }
        }

        private void SetActivationInFunctionOfScheme()
        {
            var playerInput = m_inputManagementService.GetPlayerInput();

            if (((IList)m_inputManagementService.SchemeWhereGamepadCursorIsEnable).Contains(playerInput.currentControlScheme))
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        private void UpdateSensibilityFromSettings(int sensibility)
        {
            m_virtualMouse.cursorSpeed = m_initialSpeed * sensibility;
        }

        private void UpdateMenuNavigationModeFromSettings(MenuNavigationSettings menuNavigationSettings)
        {
            m_menuNavigationSettings = menuNavigationSettings;
            ShouldBeDeactivated();
        }

        private void SetPosition(Vector3 anchoredPosition)
        {
            InputState.Change(m_virtualMouse.virtualMouse.position, anchoredPosition);
            RectTransform.anchoredPosition = anchoredPosition;
        }

        private void ClampPosition()
        {
            var pos = RectTransform.anchoredPosition;
            pos.x = Mathf.Clamp(pos.x, 0f, Screen.width);
            pos.y = Mathf.Clamp(pos.y, 0f, Screen.height);

            SetPosition(pos);
        }

        private bool ShouldBeDeactivated()
        {
            var playerInput = m_inputManagementService.GetPlayerInput();
            var canBeActivatedByScheme = ((IList)m_inputManagementService.SchemeWhereGamepadCursorIsEnable).Contains(playerInput.currentControlScheme);
            var navigationModeIsValid = m_menuNavigationSettings == MenuNavigationSettings.Cursor;

            if (!canBeActivatedByScheme || !navigationModeIsValid)
            {
                Deactivate();
                return true;
            }

            return false;
        }
    }
}
