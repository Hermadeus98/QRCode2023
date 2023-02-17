namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class InputIcon : MonoBehaviour
    {
        [SerializeField] private InputActionReference m_inputActionReference;
        [SerializeField] private PlayerInput m_playerInput;

        [SerializeField, DrawWithUnity] private InputControl m_binding;

        private string m_currentInputScheme;

        [Button]
        private void UpdateIcon()
        {
            m_currentInputScheme = m_playerInput.currentControlScheme;
            
            Debug.Log("new  scheme = " + m_currentInputScheme);
            
            var readOnlyArray = m_inputActionReference.action.bindings;

            for (int i = 0; i < readOnlyArray.Count; i++)
            {
                Debug.Log("input " + readOnlyArray[i]);
                
                if(InputControlPath.Matches(readOnlyArray[i].action, m_binding))
                {
                    Debug.Log("MATCH");
                }
            }
            
        }

        private void Update()
        {
            CheckCurrentScheme();
        }
        
        private void CheckCurrentScheme()
        {
            if (m_currentInputScheme == m_playerInput.currentControlScheme)
            {
                return;
            }
            else
            {
                UpdateIcon();
            }
        }
    }
}
