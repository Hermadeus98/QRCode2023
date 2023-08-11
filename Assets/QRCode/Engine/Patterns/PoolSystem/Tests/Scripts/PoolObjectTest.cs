namespace QRCode.Gameplay.Pooling.Tests
{
    using System;
    using System.Threading.Tasks;
    using Framework.Debugging;
    using UnityEngine;
    using LogType = Framework.Debugging.LogType;

    public class PoolObjectTest : MonoBehaviour, IPoolObject
    {
        public bool IsAvailable { get; set; }
        
        public async void OnPool()
        {
            QRDebug.DebugMessage(LogType.Debug, "Tests", "On Pool", gameObject);

            await Task.Delay(TimeSpan.FromSeconds(1f));
            this.PushObject();
        }

        public void OnPush()
        {
            QRDebug.DebugMessage(LogType.Debug, "Tests", "On Push", gameObject);
        }
    }
}
