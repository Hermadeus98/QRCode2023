namespace QRCode.Engine.Core.Audio
{
    using System;
    using UnityEngine;

    public static class AudioExtensions
    {
        public static AudioClip PlaySound(this AudioSource audioSource, SoundData soundData)
        {
            var audioClip = soundData.GetAudioClip();
            audioSource.clip = audioClip;
            audioSource.outputAudioMixerGroup = soundData.AudioMixerGroup;
            audioSource.volume = soundData.Volume;
            audioSource.pitch = soundData.Pitch;
            audioSource.priority = soundData.Priority;
            audioSource.panStereo = soundData.StereoPan;
            audioSource.spatialBlend = soundData.SpatialBlend;
            audioSource.loop = soundData.Loop;
            audioSource.bypassEffects = soundData.BypassEffects;
            audioSource.bypassListenerEffects = soundData.BypassListenerEffects;
            audioSource.bypassReverbZones = soundData.BypassReverbZone;

            switch (soundData.AudioPlayType)
            {
                case AudioPlayType.Play:
                    audioSource.Play();
                    break;
                case AudioPlayType.PlayDelayed:
                    audioSource.PlayDelayed(soundData.Delay);
                    break;
                case AudioPlayType.PlayOneShot:
                    audioSource.PlayOneShot(soundData.GetAudioClip());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return audioClip;
        }
    }
}
