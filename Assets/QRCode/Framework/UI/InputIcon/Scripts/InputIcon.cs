namespace QRCode.Framework
{
    using Debugging;
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.UI;

    public class InputIcon : SerializedMonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private bool m_preserveAspect = true;
        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private bool m_inputIconForAxis = false;
        [TitleGroup(K.InspectorGroups.Settings)] [ShowIf("m_inputIconForAxis")]
        [SerializeField] private bool positiveAxis = false;
        
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] private InputActionReference m_inputActionReference;
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] private Image m_icon;
        [TitleGroup(K.InspectorGroups.References)]
        [SerializeField] private PlayerInput m_playerInput;

        [TitleGroup(K.InspectorGroups.Debugging)]
        [SerializeField] private bool m_activateLogMessage = false;

        [TitleGroup(K.InspectorGroups.Debugging)] 
        [SerializeField] private bool m_feedbackOnPerformInput = true;
        
        private string m_currentControlScheme;
        private string m_currentDisplayName;
        private Sequence m_onPerformInputSequence;

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

        private InputSettings m_inputSettings = null;

        protected InputSettings InputSettings
        {
            get
            {
                if (m_inputSettings == null)
                {
                    m_inputSettings = InputSettings.Instance;
                }

                return m_inputSettings;
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

        private void OnEnable()
        {
            if (m_feedbackOnPerformInput)
            {
                m_icon.DOFade(.8f, 0f);
            }
            
            m_playerInput.actions[m_inputActionReference.action.name].performed += OnPerformInput;
        }

        private void OnDisable()
        {
            m_playerInput.actions[m_inputActionReference.action.name].performed -= OnPerformInput;
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
            if (positiveAxis)
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

        private void LoadIcon()
        {
            if (m_activateLogMessage)
            {
                QRDebug.Debug(K.DebuggingChannels.Inputs, $"SCHEME = {m_currentControlScheme} & INPUT = {m_currentDisplayName} for {m_inputActionReference.action.name}", InputMapDatabase);
            }

            var sanitizeControlScheme = m_currentControlScheme.Replace(" ", "");
            InputMapDatabase.TryGetInDatabase(sanitizeControlScheme, out var inputMap);
            var icon = inputMap.FindIcon(m_currentDisplayName);

            if (icon == null)
            {
                icon = InputSettings.NotFoundedIconSprite;
            }
                
            m_icon.sprite = icon;

            gameObject.name = $"Input Icon [{m_currentDisplayName}]";
            m_icon.preserveAspect = m_preserveAspect;
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

        private void OnPerformInput(InputAction.CallbackContext context)
        {
            if (m_feedbackOnPerformInput)
            {
                m_onPerformInputSequence?.Kill();
                m_onPerformInputSequence = DOTween.Sequence();
                m_onPerformInputSequence.Append(m_icon.DOFade(1f, .2f));
                m_onPerformInputSequence.Append(m_icon.DOFade(.8f, .2f).SetDelay(.2f));
                m_onPerformInputSequence.Play();
            }

            if (m_activateLogMessage)
            {
                QRDebug.Debug(K.DebuggingChannels.Inputs, $"SCHEME = {m_currentControlScheme} & INPUT = {m_currentDisplayName} for {m_inputActionReference.action.name} was performed");
            }
        }
    }
}
