namespace QRCode.Framework
{
    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.Playables;

    public class SubtitleClip : PlayableAsset
    {
        public LocalizedString SubtitleText;
        public string Text;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);

            var subtitleBehaviour = playable.GetBehaviour();
            subtitleBehaviour.SubtitleText = SubtitleText;
            subtitleBehaviour.Text = Text;

            return playable;
        }
    }
}
