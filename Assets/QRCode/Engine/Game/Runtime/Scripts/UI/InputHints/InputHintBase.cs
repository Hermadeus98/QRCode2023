namespace QRCode.Engine.Game.Inputs
{
    using System.Threading.Tasks;
    using Core.Inputs;
    using Toolbox;
    using Debugging;
    using Framework;
    using Sirenix.OdinInspector;
    using Toolbox.Database;
    using Toolbox.Database.GeneratedEnums;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.Users;

    public class InputHintBase : SerializedMonoBehaviour
    {
        [TitleGroup(Constants.InspectorGroups.Settings)] 
        [SerializeField] protected bool m_inputIconForAxis = false;
        [TitleGroup(Constants.InspectorGroups.Settings)] [ShowIf("m_inputIconForAxis")]
        [SerializeField] protected bool m_positiveAxis = false;
        [TitleGroup(Constants.InspectorGroups.Settings)] [SerializeField]
        protected int m_alternativeIconIndex = 0;
        
        [TitleGroup(Constants.InspectorGroups.References)]
        [SerializeField] protected InputActionReference m_inputActionReference;

        [TitleGroup(Constants.InspectorGroups.Debugging)]
        [SerializeField] protected bool m_activateLogMessage = false;
        [TitleGroup(Constants.InspectorGroups.Debugging)] 
        [SerializeField] protected bool m_feedbackOnPerformInput = true;
        
        [TitleGroup(Constants.InspectorGroups.Debugging)]
        [SerializeField][ReadOnly] private string m_currentControlScheme;
        [TitleGroup(Constants.InspectorGroups.Debugging)]
        [SerializeField][ReadOnly]protected string m_currentDisplayName;

        private InputMapDatabase m_inputMapDatabase;
        private IInputManagementService m_inputManagementService;
        protected PlayerInput m_playerInput;
        private string m_lastControlScheme;

        protected string CurrentControlScheme
        {
            get => m_currentControlScheme;
            private set => m_currentControlScheme = value;
        }

        protected InputMapDatabase InputMapDatabase
        {
            get
            {
                if (m_inputMapDatabase == null)
                {
                    DB.Instance.TryGetDatabase(DBEnum.DB_InputMaps, out m_inputMapDatabase);
                }

                return m_inputMapDatabase;
            }
        }

        private void Start()
        {
            m_inputManagementService = InputManager.Instance;
            m_playerInput = m_inputManagementService.GetPlayerInput();
            
            if (m_playerInput == null)
            {
                QRDebug.DebugError(Constants.DebuggingChannels.Error, $"{nameof(m_playerInput)} is null.");
                Destroy(this);
            }
        }

        protected virtual async void OnEnable()
        {
            while (m_inputManagementService == null)
            {
                await Task.Yield();
            }

            InputUser.onChange += OnInputDeviceChange;

            if (m_playerInput != null)
            {
                m_playerInput.actions[m_inputActionReference.action.name].performed -= OnPerformInput;
                m_playerInput.actions[m_inputActionReference.action.name].performed += OnPerformInput;
            }

            UpdateIcon();
        }

        protected virtual void OnDisable()
        {
            InputUser.onChange -= OnInputDeviceChange;

            if (m_playerInput != null)
            {
                m_playerInput.actions[m_inputActionReference.action.name].performed -= OnPerformInput;
            }
        }

        [Button]
        private void UpdateIcon()
        {
            //SCHEME
            CurrentControlScheme = m_playerInput.currentControlScheme;

            if (m_inputActionReference.action.bindings[0].isComposite)
            {
                if (m_inputIconForAxis == false)
                {
                    QRDebug.Debug(Constants.DebuggingChannels.Error, $"You should check {nameof(m_inputIconForAxis)} = true because action seems to be an axis.", gameObject);
                    m_inputIconForAxis = true;
                }
                LoadIconForAxis();
            }
            else
            {
                if (m_inputIconForAxis == true)
                {
                    QRDebug.Debug(Constants.DebuggingChannels.Error, $"You should check {nameof(m_inputIconForAxis)} = false because action seems to be a button.", gameObject);
                    m_inputIconForAxis = false;
                }

                LoadIconForInput();
            }
        }

        private void LoadIconForAxis()
        {
            if (m_positiveAxis)
            {
                m_currentDisplayName = m_inputActionReference.action.controls[0].displayName;
                LoadIcon();
            }
            else
            {
                m_currentDisplayName = m_inputActionReference.action.controls[1].displayName;
                LoadIcon();
            }
        }

        private void LoadIconForInput()
        {
            m_currentDisplayName = m_inputActionReference.action.GetBindingDisplayString();
            LoadIcon();
        }

        protected virtual void LoadIcon()
        {
            if (m_activateLogMessage)
            {
                QRDebug.Debug(Constants.DebuggingChannels.Inputs, $"SCHEME = {m_currentControlScheme} & INPUT = {m_currentDisplayName} for {m_inputActionReference.action.name}", InputMapDatabase);
            }
        }
        
        private void OnInputDeviceChange(InputUser user, InputUserChange change, InputDevice device) 
        {
            if (change == InputUserChange.ControlSchemeChanged) 
            {
                UpdateIcon();
            }
        }
        
        protected virtual void OnPerformInput(InputAction.CallbackContext context)
        {
            if (m_activateLogMessage)
            {
                QRDebug.Debug(Constants.DebuggingChannels.Inputs, $"SCHEME = {m_currentControlScheme} & INPUT = {m_currentDisplayName} for {m_inputActionReference.action.name}", InputMapDatabase);
            }
        }

        [Button]
        private void UpdateLongPressButton(float value)
        {
            InputSystem.settings.defaultHoldTime = 2f;
        }
    }
}
