namespace QRCode.Engine.Game.Subtitles
{
    using Toolbox;
    using Debugging;
    using UnityEngine;
    using UnityEngine.Playables;

    public class SubtitleTrackMixer : PlayableBehaviour
    {
        private SubtitleComponent m_subtitleComponent = null;

        private string m_currentSpeakerName;
        private string m_currentText;
        private float m_currentAlpha = 0f;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (CheckMissingReferences() == false)
            {
                return;
            }

            m_currentAlpha = 0f;
            m_currentSpeakerName = string.Empty;
            m_currentText = string.Empty;
            
            var inputCount = playable.GetInputCount();
            for (var i = 0; i < inputCount; i++)
            {
                var inputWeight = playable.GetInputWeight(i);

                if (inputWeight > 0f)
                {
                    ScriptPlayable<SubtitleBehaviour> inputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);
                    SubtitleBehaviour input = inputPlayable.GetBehaviour();

                    m_currentText = input.GetText();
                    m_currentSpeakerName = input.GetSpeakerName();
                    m_currentAlpha = inputWeight;
                }
            }
            
            m_subtitleComponent.SetSpeakerName(m_currentSpeakerName);
            m_subtitleComponent.SetTextRaw(m_currentText);
            m_subtitleComponent.SetTransparency(m_currentAlpha);
        }

        private bool CheckMissingReferences()
        {
            if (m_subtitleComponent == null)
            {
                if (Application.isPlaying)
                {
                    m_subtitleComponent = Subtitles.Instance.MainSubtitleComponent;
                    return false;
                }
                else
                {
#if UNITY_EDITOR
                    m_subtitleComponent = GameObject.FindObjectOfType<SubtitleComponent>();
                    return false;
#endif
                }
            }
            
            if (m_subtitleComponent == null)
            {
                QRDebug.DebugError(Constants.DebuggingChannels.Error, "Text Component cannot be found.");
                return false;
            }

            return true;
        }
    }
}
