namespace QRCode.Engine.Game.Subtitles
{
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Game.Tags;
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
            QRLogger.DebugInfo<GameTags.Subtitles>("Missing Subtitle Data");
            return Toolbox.Constants.Subtitles.SubtitleSpeakerNamePlaceHolder;
        }
        
        private string ReturnTextError()
        {
            QRLogger.DebugInfo<GameTags.Subtitles>("Missing Subtitle Data");
            return Toolbox.Constants.Subtitles.SubtitleTextPlaceHolder;
        }
    }
}
