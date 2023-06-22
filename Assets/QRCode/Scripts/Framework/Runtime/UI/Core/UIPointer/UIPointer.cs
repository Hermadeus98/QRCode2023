namespace QRCode.Framework
{
    using System.Threading.Tasks;
    using Game;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.LowLevel;
    using UnityEngine.InputSystem.Users;

    public class UIPointer : UIElement
    {
        [SerializeField][ReadOnly] private PlayerInput m_playerInput;
        [SerializeField] private float m_cursorSpeed = 1000;
        [SerializeField] private RectTransform m_canvasTransform;
        [SerializeField] private Canvas m_canvas;

        private IInputManagementService m_inputManagementService = null;

        private Camera m_camera;
        private bool m_previousMouseState;
        private Mouse m_virtualMouse;

        protected override void Start()
        {
            base.Start();
            m_inputManagementService = ServiceLocator.Current.Get<IInputManagementService>();
        }

        protected override async void OnEnable()
        {
            base.OnEnable();

            while (Bootstrap.IsInit() == false)
            {
                await Task.Yield();
            }
            
            if (m_virtualMouse == null)
            {
                m_virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            }
            else if(!m_virtualMouse.added)
            {
                InputSystem.AddDevice(m_virtualMouse);
            }

            m_playerInput = m_inputManagementService.GetPlayerInput();
            InputUser.PerformPairingWithDevice(m_virtualMouse, m_playerInput.user);
            m_camera = UICamera.Instance.Camera;
            m_inputManagementService.GetPlayerInput().camera = m_camera;

            var position = RectTransform.anchoredPosition;
            InputState.Change(m_virtualMouse, position);
            
            InputSystem.onAfterUpdate += UpdateMotion;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            InputSystem.RemoveDevice(m_virtualMouse);
            InputSystem.onAfterUpdate -= UpdateMotion;
        }

        private void UpdateMotion()
        {
            if (m_virtualMouse == null || Gamepad.current == null)
            {
                return;
            }

            var deltaValue = Gamepad.current.leftStick.ReadValue();
            deltaValue *= m_cursorSpeed * Time.unscaledDeltaTime;

            var currentPosition = m_virtualMouse.position.ReadValue();
            var newPosition = currentPosition + deltaValue;

            newPosition.x = Mathf.Clamp(newPosition.x, 0f, Screen.width);
            newPosition.y = Mathf.Clamp(newPosition.y, 0f, Screen.height);
            
            InputState.Change(m_virtualMouse.position, newPosition);
            InputState.Change(m_virtualMouse.delta, deltaValue);

            var aButtonIsPressed = Gamepad.current.aButton.IsPressed();
            if (m_previousMouseState != Gamepad.current.aButton.isPressed)
            {
                m_virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, Gamepad.current.aButton.IsPressed());
                InputState.Change(m_virtualMouse, mouseState);
                m_previousMouseState = aButtonIsPressed;
            }
            
            AnchorCursor(newPosition);
        }

        private void AnchorCursor(Vector2 position)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvasTransform, position, m_canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_camera, out var anchoredPosition);
            RectTransform.anchoredPosition = anchoredPosition;
        }
    }
}
