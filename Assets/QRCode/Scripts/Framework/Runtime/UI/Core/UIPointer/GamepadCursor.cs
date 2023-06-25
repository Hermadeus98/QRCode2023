namespace QRCode.Framework
{
    using System.Collections;
    using System.Threading.Tasks;
    using Events;
    using Game;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.LowLevel;
    using UnityEngine.InputSystem.UI;
    using UnityEngine.InputSystem.Users;

    public class GamepadCursor : UIElement
    {
        [TitleGroup(K.InspectorGroups.Settings)]
        [SerializeField] private bool m_isMain = false;

        [TitleGroup(K.InspectorGroups.References)] 
        [SerializeField] private VirtualMouseInput m_virtualMouse;
        
        private static GamepadCursor Main;
        private float m_initialSpeed;
        private IInputManagementService m_inputManagementService = null;

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
                    m_userSettingsData = ServiceLocator.Current.Get<IUserSettingsService>().GetUserSettingsData();
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
            
            m_inputManagementService = ServiceLocator.Current.Get<IInputManagementService>();

            base.Start();
            m_initialSpeed = m_virtualMouse.cursorSpeed;
        }

        private void Update()
        {
            ClampPosition();
        }

        protected override async void OnEnable()
        {
            base.OnEnable();
            
            GamepadCursorSensibilityEvent.Register(UpdateSensibilityFromSettings);
            Init();
        }

        private async void Init()
        {
            while (Bootstrap.IsInit() == false)
            {
                await Task.Yield();
            }
            
            UpdateSensibilityFromSettings(UserSettingsData.GamepadCursorSensibility);
            InputUser.onChange += OnControlsChanged;
            
            SetActivationInFunctionOfScheme();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GamepadCursorSensibilityEvent.Unregister(UpdateSensibilityFromSettings);
            InputUser.onChange -= OnControlsChanged;
        }

        public void Activate()
        {
            CanvasGroup.alpha = 1f;
        }

        public void Deactivate()
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

        public void SetPosition(Vector3 anchoredPosition)
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
    }
}
