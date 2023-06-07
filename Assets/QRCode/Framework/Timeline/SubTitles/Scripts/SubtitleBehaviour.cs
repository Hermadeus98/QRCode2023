namespace QRCode.Framework
{
    using Debugging;
    using UnityEngine;
    using UnityEngine.Playables;

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
            QRDebug.DebugInfo(K.DebuggingChannels.Subtitles, "Missing Subtitle Data");
            return K.Subtitles.SubtitleSpeakerNamePlaceHolder;
        }
        
        private string ReturnTextError()
        {
            QRDebug.DebugInfo(K.DebuggingChannels.Subtitles, "Missing Subtitle Data");
            return K.Subtitles.SubtitleTextPlaceHolder;
        }
    }
}
