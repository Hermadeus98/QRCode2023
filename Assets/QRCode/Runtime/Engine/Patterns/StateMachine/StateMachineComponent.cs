namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    [RequireComponent(typeof(BlackBoard))]
    public class StateMachineComponent : SerializedMonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private string m_firstStateName;
        [SerializeField] private IState[] m_states;
        [SerializeField] private UpdateModeEnum m_updateModeEnum = UpdateModeEnum.Update;

        private StateMachine m_stateMachine;

        [Button]
        public void Initialize()
        {
            m_stateMachine = new StateMachine(m_firstStateName, m_updateModeEnum, m_states);
        }

        [Button]
        public void SetState(string stateName)
        {
            m_stateMachine.SetState(stateName);
        }

        [Button]
        public void Pause(bool value)
        {
            m_stateMachine.Pause(value);
        }
    }
}
