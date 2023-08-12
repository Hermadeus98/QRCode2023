namespace QRCode.Engine.Core.Editor.Importer
{
    using UnityEditor;
    using UnityEngine;

    public class AudioCustomImporter : AssetPostprocessor
    {
        private const string m_SFXPrefix = "S_SFX_";
        private const string m_musicPrefix = "S_M_";
        private const string m_ambientPrefix = "S_A_";
        private const string m_voice_Prefix = "S_V_";
        private const string m_UIPrefix = "S_UI_";

        private void OnPreprocessAudio()
        {
            var audioImporter = (AudioImporter)assetImporter;

            if (audioImporter.importSettingsMissing == true)
            {
                Debug.Log("Import DONE");

                if (assetPath.Contains(m_SFXPrefix))
                {
                    audioImporter.forceToMono = true;
                    audioImporter.preloadAudioData = true;
                    audioImporter.loadInBackground = false;
                    audioImporter.ambisonic = false;

                    audioImporter.defaultSampleSettings = new AudioImporterSampleSettings()
                    {
                        compressionFormat = AudioCompressionFormat.PCM,
                        loadType = AudioClipLoadType.DecompressOnLoad,
                        sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate,
                        sampleRateOverride = 44100,
                    };
                }

                if (assetPath.Contains(m_musicPrefix))
                {
                    audioImporter.forceToMono = false;
                    audioImporter.preloadAudioData = false;
                    audioImporter.loadInBackground = false;
                    audioImporter.ambisonic = false;

                    audioImporter.defaultSampleSettings = new AudioImporterSampleSettings()
                    {
                        compressionFormat = AudioCompressionFormat.Vorbis,
                        quality = 70,
                        loadType = AudioClipLoadType.Streaming,
                        sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate,
                        sampleRateOverride = 44100,
                    };
                }
                
                if (assetPath.Contains(m_ambientPrefix))
                {
                    audioImporter.forceToMono = true;
                    audioImporter.preloadAudioData = true;
                    audioImporter.loadInBackground = false;
                    audioImporter.ambisonic = false;

                    audioImporter.defaultSampleSettings = new AudioImporterSampleSettings()
                    {
                        compressionFormat = AudioCompressionFormat.ADPCM,
                        loadType = AudioClipLoadType.DecompressOnLoad,
                        sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate,
                        sampleRateOverride = 44100,
                    };
                }
                
                if (assetPath.Contains(m_voice_Prefix))
                {
                    audioImporter.forceToMono = true;
                    audioImporter.preloadAudioData = true;
                    audioImporter.loadInBackground = false;
                    audioImporter.ambisonic = false;

                    audioImporter.defaultSampleSettings = new AudioImporterSampleSettings()
                    {
                        compressionFormat = AudioCompressionFormat.ADPCM,
                        loadType = AudioClipLoadType.DecompressOnLoad,
                        sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate,
                        sampleRateOverride = 44100,
                    };
                }
                
                if (assetPath.Contains(m_UIPrefix))
                {
                    audioImporter.forceToMono = true;
                    audioImporter.preloadAudioData = true;
                    audioImporter.loadInBackground = false;
                    audioImporter.ambisonic = false;

                    audioImporter.defaultSampleSettings = new AudioImporterSampleSettings()
                    {
                        compressionFormat = AudioCompressionFormat.PCM,
                        loadType = AudioClipLoadType.DecompressOnLoad,
                        sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate,
                        sampleRateOverride = 44100,
                    };
                }
            }
        }
    }
}
