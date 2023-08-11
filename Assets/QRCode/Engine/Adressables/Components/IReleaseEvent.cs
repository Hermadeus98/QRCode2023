namespace QRCode.Framework
{
    using System;

    public interface IReleaseEvent
    {
        event Action Dispatched;
    }
}