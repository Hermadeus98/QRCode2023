namespace QRCode.Framework.Observer
{
    using System;
    using System.Collections.Generic;

    public abstract class Observer
    {
        
    }

    public class Observer<T> : Observer, IDisposable
    {
        private List<IObservable<T>> m_allObservables = new List<IObservable<T>>();

        public Observer()
        {
            Observers.RegisterObserver(this);
        }
        
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

        public void Dispose()
        {
            Observers.UnregisterObserver(this);
        }
    }
}
