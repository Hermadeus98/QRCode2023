namespace QRCode.Framework
{
    using Debugging;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class InputIconBase : SerializedMonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] protected bool m_inputIconForAxis = false;
        [TitleGroup(K.InspectorGroups.Settings)] [ShowIf("m_inputIconForAxis")]
        [SerializeField] protected bool m_positiveAxis = false;
        [TitleGroup(K.InspectorGroups.Settings)] [SerializeField]
        protected int m_alternativeIconIndex = 0;
        
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] protected InputActionReference m_inputActionReference;
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] protected PlayerInput m_playerInput;

        [TitleGroup(K.InspectorGroups.Debugging)]
        [SerializeField] protected bool m_activateLogMessage = false;
        [TitleGroup(K.InspectorGroups.Debugging)] 
        [SerializeField] protected bool m_feedbackOnPerformInput = true;
        
        protected string m_currentControlScheme;
        protected string m_currentDisplayName;

        private InputMapDatabase m_inputMapDatabase = null;

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
            if (m_playerInput == null)
            {
                QRDebug.DebugError(K.DebuggingChannels.Error, $"{nameof(m_playerInput)} is null.");
                Destroy(this);
            }
        }

        protected virtual void OnEnable()
        {
            m_playerInput.actions[m_inputActionReference.action.name].performed += OnPerformInput;
        }

        private void OnDisable()
        {
            if (m_playerInput != null)
            {
                m_playerInput.actions[m_inputActionReference.action.name].performed -= OnPerformInput;
            }
        }

        private void Update()
        {
            CheckCurrentScheme();
        }

        [Button]
        private void UpdateIcon()
        {
            //SCHEME
            m_currentControlScheme = m_playerInput.currentControlScheme;

            if (m_inputActionReference.action.bindings[0].isComposite)
            {
                if (m_inputIconForAxis == false)
                {
                    QRDebug.Debug(K.DebuggingChannels.Error, $"You should check {nameof(m_inputIconForAxis)} = true because action seems to be an axis.", gameObject);
                    m_inputIconForAxis = true;
                }
                LoadIconForAxis();
            }
            else
            {
                if (m_inputIconForAxis == true)
                {
                    QRDebug.Debug(K.DebuggingChannels.Error, $"You should check {nameof(m_inputIconForAxis)} = false because action seems to be a button.", gameObject);
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
                QRDebug.Debug(K.DebuggingChannels.Inputs, $"SCHEME = {m_currentControlScheme} & INPUT = {m_currentDisplayName} for {m_inputActionReference.action.name}", InputMapDatabase);
            }
        }

        private void CheckCurrentScheme()
        {
            if (m_currentControlScheme == m_playerInput.currentControlScheme)
            {
                return;
            }
            else
            {
                UpdateIcon();
            }
        }

        protected virtual void OnPerformInput(InputAction.CallbackContext context)
        {
            if (m_activateLogMessage)
            {
                QRDebug.Debug(K.DebuggingChannels.Inputs, $"SCHEME = {m_currentControlScheme} & INPUT = {m_currentDisplayName} for {m_inputActionReference.action.name}", InputMapDatabase);
            }
        }
    }
}
