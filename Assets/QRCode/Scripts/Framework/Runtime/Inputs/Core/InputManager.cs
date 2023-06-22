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
    }
}
