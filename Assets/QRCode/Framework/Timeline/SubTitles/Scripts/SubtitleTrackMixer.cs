namespace QRCode.Framework
{
    using Debugging;
    using UnityEngine;
    using UnityEngine.Playables;

    public class SubtitleTrackMixer : PlayableBehaviour
    {
        private SubtitleComponent m_subtitleComponent = null;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (CheckMissingReferences() == false)
            {
                return;
            }

            var currentAlpha = 0f;
            var currentText = "";
            
            var inputCount = playable.GetInputCount();
            for (var i = 0; i < inputCount; i++)
            {
                var inputWeight = playable.GetInputWeight(i);

                if (inputWeight > 0f)
                {
                    ScriptPlayable<SubtitleBehaviour> inputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);
                    SubtitleBehaviour input = inputPlayable.GetBehaviour();

                    if (input.SubtitleText.IsEmpty)
                    {
                        currentText = input.Text;
                    }
                    else
                    {
                        currentText = input.SubtitleText.GetLocalizedString();
                    }

#if UNITY_EDITOR
                    if (!Application.isPlaying)
                    {
                        currentText = input.Text;
                    }
#endif

                    currentAlpha = inputWeight;
                }
            }
            
            m_subtitleComponent.SetTextRaw(currentText);
            m_subtitleComponent.SetTransparency(currentAlpha);
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
                QRDebug.DebugError(K.DebuggingChannels.Error, "Text Component cannot be found.");
                return false;
            }

            return true;
        }
    }
}
