namespace QRCode.Engine
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class DebugInformationView : MonoBehaviour
    {
        private CanvasGroup m_canvasGroup = null;
        private bool m_isShow = false;

        private void Start()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (Gamepad.current != null)
            {
                if (Gamepad.current.rightStickButton.wasPressedThisFrame || Keyboard.current.tabKey.wasPressedThisFrame)
                {
                    m_isShow = !m_isShow;
                    m_canvasGroup.alpha = m_isShow ? 1f : 0f;
                }
            }
        }

        private void OnDestroy()
        {
            m_canvasGroup = null;
        }
    }
}
