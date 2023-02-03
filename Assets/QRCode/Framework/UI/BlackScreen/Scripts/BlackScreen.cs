namespace QRCode.Framework
{
    using UnityEngine;

    public class BlackScreen : UIView, ILoadingScreen
    {
        public void Progress(float progression)
        {
            Debug.Log(progression);
        }
    }
}
