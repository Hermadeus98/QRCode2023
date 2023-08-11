namespace QRCode.Framework.Observer
{
    using System;
    using Sirenix.OdinInspector;
    using Wrappers;

    public class ObserverTest : SerializedMonoBehaviour
    {
        private Observer<EventArgs> m_floatObserver = null;

        private void Awake()
        {
           // m_floatObserver = Observers.GetObserver<EventArgs>();
        }

        [Button]
        private void Notify()
        {
            m_floatObserver.NotifyAllObservable(new WrapperArgs<float>(5f));
        }
    }
}
