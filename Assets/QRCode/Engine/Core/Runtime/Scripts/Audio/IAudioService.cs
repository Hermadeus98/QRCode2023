namespace QRCode.Engine.Core.Audio
{
    using Toolbox.Pattern.Service;
    using UnityEngine;

    public interface IAudioService : IService
    {
        public void PlaySound(SoundData soundData);
        public void PlaySound(SoundData soundData, Transform target);
    }
}
