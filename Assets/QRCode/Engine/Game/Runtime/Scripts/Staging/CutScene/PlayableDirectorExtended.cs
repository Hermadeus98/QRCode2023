namespace QRCode.Engine.Toolbox
{
    using UnityEngine;
    using UnityEngine.Playables;

    [RequireComponent(typeof(PlayableDirector))]
    public class PlayableDirectorExtended : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _playableDirector = null;

        private bool _isPlaying = false;
        private float _startTime = 0.0f;

        public void Play()
        {
            _isPlaying = true;
        }

        public void Stop()
        {
            _isPlaying = false;
        }

        public void Pause()
        {
            
        }

        public void Resume()
        {
            
        }
    }
}
