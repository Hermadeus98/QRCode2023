namespace QRCode.Framework
{
    using Debugging;
    using UnityEngine;
    using UnityEngine.Playables;

    public class SubtitleTrackMixer : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var subtitleComponent = playerData as SubtitleComponent;

            if (subtitleComponent == null)
            {
                QRDebug.DebugError(K.DebuggingChannels.Error, "Text Component is missing as binding.");
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
            
            subtitleComponent.SetTextRaw(currentText);
            subtitleComponent.SubtitleCanvasGroup.alpha = currentAlpha;
        }
    }
}
