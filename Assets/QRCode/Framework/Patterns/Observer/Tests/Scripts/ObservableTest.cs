namespace QRCode.Framework
{
    using Debugging;
    using UnityEngine;
    using Wrappers;
    using EventArgs = System.EventArgs;
    using LogType = Debugging.LogType;

    public class ObservableTest : MonoBehaviour, Observer.IObservable<EventArgs>
    {
        private void OnEnable()
        {
            //Observers.GetObserver<EventArgs>().RegisterObservable(this);
        }

        private void OnDisable()
        {
            //Observers.GetObserver<EventArgs>().UnregisterObservable(this);
        }

        public void OnNotify(EventArgs arg)
        {
            if (arg is WrapperArgs<float> wrapper)
            {
                QRDebug.DebugMessage(LogType.Debug, "Editor", $"{wrapper.Arg}", gameObject);
            }
        }
    }
}
