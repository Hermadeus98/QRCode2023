namespace QRCode.Engine.Game.Subtitles
{
    using Debugging;
    using UnityEngine;
    using UnityEngine.Playables;
    using Toolbox;
    using Constants = Toolbox.Constants;

    public class SubtitleBehaviour : PlayableBehaviour
    {
        public SubtitleData SubtitleData { get; set; }

        public string GetSpeakerName()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                if (SubtitleData != null)
                {
                    return SubtitleData.GetSubtitleSpeakerNameForPlaceHolder();
                }
                else
                {
                    return ReturnSpeakerNameError();
                }
            }
#endif
            
            if (SubtitleData != null)
            {
                return SubtitleData.GetSpeakerName();
            }

            return ReturnSpeakerNameError();
        }
        
        public string GetText()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                if (SubtitleData != null)
                {
                    return SubtitleData.GetSubtitleTextForPlaceHolder();
                }
                else
                {
                    return ReturnTextError();
                }
            }
#endif
            
            if (SubtitleData != null)
            {
                return SubtitleData.GetText();
            }

            return ReturnTextError();
        }

        private string ReturnSpeakerNameError()
        {
            QRDebug.DebugInfo(Constants.DebuggingChannels.Subtitles, "Missing Subtitle Data");
            return Constants.Subtitles.SubtitleSpeakerNamePlaceHolder;
        }
        
        private string ReturnTextError()
        {
            QRDebug.DebugInfo(Constants.DebuggingChannels.Subtitles, "Missing Subtitle Data");
            return Constants.Subtitles.SubtitleTextPlaceHolder;
        }
    }
}
