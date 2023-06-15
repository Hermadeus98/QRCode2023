namespace QRCode.Framework
{
    using System;
    using UnityEngine;

    public class ReleaseAddressableInstanceEvent : MonoBehaviour, IReleaseEvent
    {
        private void OnDestroy()
        {
            ReleasedInternal?.Invoke();
        }

        event Action IReleaseEvent.Dispatched
        {
            add => ReleasedInternal += value;
            remove => ReleasedInternal -= value;
        }

        private event Action ReleasedInternal;
    }
}
