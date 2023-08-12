namespace QRCode.Engine.Toolbox.Pattern.Observable
{
    using System.Collections.Generic;

    public class Observer
    {
        private List<IObservable> m_allObservables = new List<IObservable>();
        
        
        public void RegisterObservable(IObservable observable)
        {
            m_allObservables.Add(observable);
        }

        public void UnregisterObservable(IObservable observable)
        {
            m_allObservables.Remove(observable);
        }

        public void NotifyAllObservable()
        {
            for (int i = 0; i < m_allObservables.Count; i++)
            {
                m_allObservables[i].OnNotify();
            }
        }
    }

    public class Observer<T>
    {
        private List<IObservable<T>> m_allObservables = new List<IObservable<T>>();

        public void RegisterObservable(IObservable<T> observable)
        {
            m_allObservables.Add(observable);
        }

        public void UnregisterObservable(IObservable<T> observable)
        {
            m_allObservables.Remove(observable);
        }

        public void NotifyAllObservable(T argument)
        {
            for (int i = 0; i < m_allObservables.Count; i++)
            {
                m_allObservables[i].OnNotify(argument);
            }
        }
    }
}
