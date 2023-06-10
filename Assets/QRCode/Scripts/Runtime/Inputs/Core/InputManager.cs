namespace QRCode.Framework
{
    using Debugging;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class InputManager : SerializedMonoBehaviour, IInputManagementService
    {
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] protected PlayerInput m_playerInput;

        public PlayerInput GetPlayerInput()
        {
            return m_playerInput;
        }

        [Button]
        public void SetActionMapEnable(string actionMap)
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Inputs, $"Action Map [{actionMap}] is enable", gameObject);
            m_playerInput.actions.FindActionMap(actionMap).Enable();
        }

        [Button]
        public void SetActionMapDisable(string actionMap)
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Inputs, $"Action Map [{actionMap}] is disable", gameObject);
            m_playerInput.actions.FindActionMap(actionMap).Disable();
        }
    }
}
