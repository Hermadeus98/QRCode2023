namespace QRCode.Framework
{
    public class Subtitles
    {
        private static Subtitles m_instance;
        public static Subtitles Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new Subtitles();
                }

                return m_instance;
            }
        }

        private SubtitleComponent m_mainSubtitleComponent;
        public SubtitleComponent MainSubtitleComponent => m_mainSubtitleComponent;

        public void SetMainSubtitleComponent(SubtitleComponent subtitleComponent)
        {
            m_mainSubtitleComponent = subtitleComponent;
        }
    }
}