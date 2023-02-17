namespace QRCode.Framework
{
    using UnityEngine;

    public class State : IState
    {
        [SerializeField] protected string m_stateName;
        
        public string StateName
        {
            get => m_stateName;
            set => m_stateName = value;
        }

        public void OnStateEnter()
        {
            
        }

        public void OnStateUpdate()
        {
            
        }

        public void OnStateExit()
        {
            
        }
    }
}
