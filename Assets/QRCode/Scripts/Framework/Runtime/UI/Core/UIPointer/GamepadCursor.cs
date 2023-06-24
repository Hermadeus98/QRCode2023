namespace QRCode.Framework
{
    using Events;
    using Game;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.LowLevel;
    using UnityEngine.InputSystem.UI;

    public class GamepadCursor : UIElement
    {
        [TitleGroup(K.InspectorGroups.Settings)]
        [SerializeField] private bool m_isMain = false;
        
        [TitleGroup(K.InspectorGroups.References)] 
        [SerializeField] private VirtualMouseInput m_virtualMouse;
        
        private static GamepadCursor Main;
        private float m_initialSpeed;
        private IInputManagementService m_inputManagementService = null;
        
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
            m_inputManagementService = ServiceLocator.Current.Get<IInputManagementService>();

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

            if (Bootstrap.IsInit())
            {
                UpdateSensibilityFromSettings(UserSettingsData.GamepadCursorSensibility);
            }
            
            //m_inputManagementService.GetPlayerInput().onControlsChanged += OnControlsChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GamepadCursorSensibilityEvent.Unregister(UpdateSensibilityFromSettings);
            //m_inputManagementService.GetPlayerInput().onControlsChanged -= OnControlsChanged;
        }

        public void Activate()
        {
            CanvasGroup.alpha = 1f;
        }

        public void Deactivate()
        {
            CanvasGroup.alpha = 0f;
        }
        
        private void OnControlsChanged(PlayerInput obj)
        {
            if (obj.currentControlScheme == "KeyboardAndMouse")
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
