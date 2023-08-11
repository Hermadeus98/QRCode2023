namespace QRCode.Framework
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "STG_AudioSettings", menuName = K.SettingsPath.AudioSettingsPath, order = 0)]
    public class AudioSettings : Settings<AudioSettings>
    {
        [SerializeField] private AudioHandler m_audioHandlerPrefab = null;

        public AudioHandler AudioHandlerPrefab => m_audioHandlerPrefab;
    }
}