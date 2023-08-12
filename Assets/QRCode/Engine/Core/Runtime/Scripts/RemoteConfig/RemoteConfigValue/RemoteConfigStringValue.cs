namespace QRCode.Engine
{
    using Unity.Services.RemoteConfig;
    using UnityEngine;
    
    using Debugging;

    [CreateAssetMenu(menuName = "QRCode/Engine/Remote Config/Remote Config Value/String Remote Config Value", fileName = "RCV_NewStringRemoteConfigValue")]
    public class RemoteConfigStringValue : RemoteConfigValueBase<string>
    {
        public override void FetchValue(RuntimeConfig runtimeConfig)
        {
            if (runtimeConfig.HasKey(m_key))
            {
                m_value = runtimeConfig.GetString(m_key, m_defaultValue);
            }
            else
            {
                m_value = m_defaultValue;
                QRDebug.DebugError(Toolbox.Constants.DebuggingChannels.RemoteConfig, $"{nameof(runtimeConfig)} don't contain key for [{m_key}] in {name}. <b>Value</> will be fill with <n>default value</> : {m_defaultValue}.", this);
            }
        }
    }
}