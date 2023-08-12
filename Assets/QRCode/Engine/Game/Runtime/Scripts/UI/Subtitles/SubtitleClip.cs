namespace QRCode.Engine.Game.Subtitles
{
    using Toolbox;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Playables;

    public class SubtitleClip : PlayableAsset
    {
        [TitleGroup(Constants.InspectorGroups.References)] 
        [SerializeField] private SubtitleData m_subtitleData = null;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);

            var subtitleBehaviour = playable.GetBehaviour();
            subtitleBehaviour.SubtitleData = m_subtitleData;

            return playable;
        }
    }
}
