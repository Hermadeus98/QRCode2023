namespace QRCode.Engine.Toolbox
{
    using System;

    public class EventOneShot
    {
        private Action m_action;

        public void Register(Action action)
        {
            m_action += action.Invoke;
        }

        public void Unregister(Action action)
        {
            m_action -= action.Invoke;
        }

        public void Invoke()
        {
            m_action.Invoke();
            
            var allDelegates = m_action.GetInvocationList();
            foreach (var dDelegate in allDelegates)
            {
                m_action -= (Action)dDelegate;
            }
        }
    }
}
