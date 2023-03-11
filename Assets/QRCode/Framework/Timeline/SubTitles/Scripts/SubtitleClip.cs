namespace QRCode.Framework
{
    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.Playables;

    public class SubtitleClip : PlayableAsset
    {
        public LocalizedString SubtitleText;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);

            var subtitleBehaviour = playable.GetBehaviour();
            subtitleBehaviour.SubtitleText = SubtitleText;

            return playable;
        }
    }
}
