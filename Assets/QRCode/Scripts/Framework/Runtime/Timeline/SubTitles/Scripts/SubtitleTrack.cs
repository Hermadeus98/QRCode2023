namespace QRCode.Framework
{
    using UnityEngine;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;

    [TrackClipType(typeof(SubtitleClip))]
    public class SubtitleTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<SubtitleTrackMixer>.Create(graph, inputCount);
        }
    }
}