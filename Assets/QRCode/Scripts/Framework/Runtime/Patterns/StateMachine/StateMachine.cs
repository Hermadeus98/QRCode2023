namespace QRCode.Framework
{
    using System;
    using System.Collections.Generic;
    using Debugging;
    using UnityEngine;

    [Serializable]
    public class StateMachine
    {
        [SerializeField] private UpdateModeEnum m_updateModeEnum;
        
        private IState m_currentState = null;
        private List<IState> m_allStates = new List<IState>();
        private bool m_isInPause = false;
        
        public StateMachine(string firstState, UpdateModeEnum updateModeEnum, IState[] states)
        {
            m_updateModeEnum = updateModeEnum;
            
            AddStates(states);
            SetState(firstState);
        }

        public void AddStates(params IState[] states)
        {
            m_allStates.AddRange(states);
        }
        
        public void SetState(string stateName)
        {
            var newState = GetState(stateName);

            if (newState == null)
            {
                QRDebug.DebugError(K.DebuggingChannels.Error, $"Cannot found the state {stateName} in state machine.");
                return;
            }
            
            m_currentState?.OnStateExit();
            m_currentState = newState;
            m_currentState.OnStateEnter();
        }

        public IState GetState(string stateName)
        {
            for (int i = 0; i < m_allStates.Count; i++)
            {
                if (m_allStates[i].StateName == stateName)
                    return m_allStates[i];
            }

            return null;
        }
        
        public void Pause(bool value)
        {
            m_isInPause = value;
        }
        
        private void UpdateCurrentState()
        {
            if(m_isInPause)
                return;
            
            if (m_currentState != null)
            {
                m_currentState.OnStateUpdate();
            }
        }
    }
}
