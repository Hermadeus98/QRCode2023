namespace QRCode.Framework
{
    using Debugging;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class StateTest : MonoBehaviour, IState
    {
        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private string m_stateName;

        public string StateName
        {
            get => m_stateName; 
            set => m_stateName = value;
        }
        
        public void OnStateEnter()
        {
            QRDebug.Debug(K.DebuggingChannels.Tests, "Enter : " + m_stateName);
        }

        public void OnStateUpdate()
        {
            QRDebug.Debug(K.DebuggingChannels.Tests, "Update : " + m_stateName);
        }

        public void OnStateExit()
        {
            QRDebug.Debug(K.DebuggingChannels.Tests, "Exit : " + m_stateName);
        }
    }
}
