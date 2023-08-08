namespace QRCode.Framework
{
    using UnityEngine;

    public interface IAudioService : IService
    {
        public void PlaySound(SoundData soundData);
        public void PlaySound(SoundData soundData, Transform target);
    }
}
