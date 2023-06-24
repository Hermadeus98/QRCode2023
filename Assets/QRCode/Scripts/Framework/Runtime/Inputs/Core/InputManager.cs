namespace QRCode.Framework
{
    using System.Linq;
    using Debugging;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.Users;

    public class InputManager : SerializedMonoBehaviour, IInputManagementService
    {
        [TitleGroup(K.InspectorGroups.Settings)] [SerializeField]
        private string[] m_schemeWhereMouseIsEnable = new[] { "KeyboardAndMouse" };
        
        [TitleGroup(K.InspectorGroups.Settings)] [SerializeField]
        private string[] m_schemeWhereGamepadCursorIsEnable = new[] { "Gamepad" };
        
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] protected PlayerInput m_playerInput;

        private InputMapGroupDatabase m_mapGroupDatabase = null;
        private InputMapGroupDatabase MapGroupDatabase
        {
            get
            {
                if (m_mapGroupDatabase == null)
                {
                    DB.Instance.TryGetDatabase<InputMapGroupDatabase>(DBEnum.DB_InputMapGroup, out m_mapGroupDatabase);
                }

                return m_mapGroupDatabase;
            }
        }

        public string[] SchemeWhereMouseIsEnable => m_schemeWhereMouseIsEnable;
        public string[] SchemeWhereGamepadCursorIsEnable => m_schemeWhereGamepadCursorIsEnable;

        private void Start()
        {
            InputUser.onChange += InputUserOnChange;
            
            Application.quitting += ApplicationOnQuitting;
        }

        public PlayerInput GetPlayerInput()
        {
            return m_playerInput;
        }

        [Button]
        public void SetActionMapEnable(string actionMap)
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Inputs, $"Action Map [{actionMap}] is enable.", gameObject);
            m_playerInput.actions.FindActionMap(actionMap).Enable();
        }

        [Button]
        public void SetActionMapDisable(string actionMap)
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Inputs, $"Action Map [{actionMap}] is disable.", gameObject);
            m_playerInput.actions.FindActionMap(actionMap).Disable();
        }

        [Button]
        public void SetActionMapGroupEnable(DB_InputMapGroupEnum inputMapGroup)
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Inputs, $"Action Map Group [{inputMapGroup.ToString()}] is enable.", gameObject);
            if (MapGroupDatabase.TryGetInDatabase(inputMapGroup.ToString(), out var inputMapGroupData))
            {
                foreach (var actionMap in inputMapGroupData.ActionMaps)
                {
                    m_playerInput.actions.FindActionMap(actionMap).Enable();
                }
            }
        }

        [Button]
        public void SetActionMapGroupDisable(DB_InputMapGroupEnum inputMapGroup)
        {
            QRDebug.DebugTrace(K.DebuggingChannels.Inputs, $"Action Map Group [{inputMapGroup.ToString()}] is disable.", gameObject);
            if (MapGroupDatabase.TryGetInDatabase(inputMapGroup.ToString(), out var inputMapGroupData))
            {
                foreach (var actionMap in inputMapGroupData.ActionMaps)
                {
                    m_playerInput.actions.FindActionMap(actionMap).Disable();
                }
            }
        }
        
        private void ApplicationOnQuitting()
        {
            InputUser.onChange -= InputUserOnChange;
            Application.quitting -= ApplicationOnQuitting;
        }

        private void InputUserOnChange(InputUser inputUser, InputUserChange inputUserChange, InputDevice inputDevice)
        {
            if (inputUserChange == InputUserChange.ControlsChanged)
            {
                //TO DO - Better management for visible state (depend on game state and more)
                Cursor.visible = SchemeWhereMouseIsEnable.Contains(m_playerInput.currentControlScheme);
            }
        }
    }
}
