namespace QRCode.Engine.Toolbox.AddressableManagement
{
    using System;

    public interface IReleaseEvent
    {
        event Action Dispatched;
    }
}