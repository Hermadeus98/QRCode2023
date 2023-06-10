namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Playables;

    public class SubtitleClip : PlayableAsset
    {
        [TitleGroup(K.InspectorGroups.References)] 
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
