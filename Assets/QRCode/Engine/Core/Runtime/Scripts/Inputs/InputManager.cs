namespace QRCode.Engine.Core.Inputs
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.Inputs.GeneratedEnums;
    using QRCode.Engine.Core.Manager;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Toolbox.Database;
    using QRCode.Engine.Toolbox.Database.GeneratedEnums;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.Users;

    public class InputManager : GenericManagerBase<InputManager>
    {
        [TitleGroup(Toolbox.Constants.InspectorGroups.Settings)] [SerializeField]
        private string[] m_schemeWhereMouseIsEnable = new[] { "KeyboardAndMouse" };
        
        [TitleGroup(Toolbox.Constants.InspectorGroups.Settings)] [SerializeField]
        private string[] m_schemeWhereGamepadCursorIsEnable = new[] { "Gamepad" };
        
        [TitleGroup(Toolbox.Constants.InspectorGroups.References)]
        [SerializeField] protected PlayerInput m_playerInput;

        private InputMapGroupDatabase m_mapGroupDatabase = null;
        private InputMapGroupDatabase MapGroupDatabase
        {
            get
            {
                m_mapGroupDatabase = DB.Instance.GetDatabase<InputMapGroupDatabase>(DBEnum.DB_InputMapGroup);
                return m_mapGroupDatabase;
            }
        }

        public string[] SchemeWhereMouseIsEnable => m_schemeWhereMouseIsEnable;
        public string[] SchemeWhereGamepadCursorIsEnable => m_schemeWhereGamepadCursorIsEnable;

        protected override Task InitAsync(CancellationToken cancellationToken)
        {
            InputUser.onChange += InputUserOnChange;
            
            Application.quitting += ApplicationOnQuitting;
            
            return Task.CompletedTask;
        }

        public override void Delete()
        {
            base.Delete();
        }

        public PlayerInput GetPlayerInput()
        {
            return m_playerInput;
        }

        [Button]
        public void SetActionMapEnable(string actionMap)
        {
            QRLogger.DebugTrace<CoreTags.Inputs>($"Action Map [{actionMap}] is enable.", gameObject);
            m_playerInput.actions.FindActionMap(actionMap).Enable();
        }

        [Button]
        public void SetActionMapDisable(string actionMap)
        {
            QRLogger.DebugTrace<CoreTags.Inputs>($"Action Map [{actionMap}] is disable.", gameObject);
            m_playerInput.actions.FindActionMap(actionMap).Disable();
        }

        [Button]
        public void SetActionMapGroupEnable(DB_InputMapGroupEnum inputMapGroup)
        {
            QRLogger.DebugTrace<CoreTags.Inputs>($"Action Map Group [{inputMapGroup.ToString()}] is enable.", gameObject);
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
            QRLogger.DebugTrace<CoreTags.Inputs>($"Action Map Group [{inputMapGroup.ToString()}] is disable.", gameObject);
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
