namespace QRCode.Engine.Core.Tests
{
    using Debugging;
    using Constants = Toolbox.Constants;

    public class GameplayComponentExample : GameplayComponent
    {
        private void Awake()
        {
            QRDebug.DebugInfo(Constants.DebuggingChannels.LifeCycle, "TEST -> AWAKE");
        }

        private void Start()
        {
            QRDebug.DebugInfo(Constants.DebuggingChannels.LifeCycle, "TEST -> START");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            QRDebug.DebugInfo(Constants.DebuggingChannels.LifeCycle, "TEST -> ON_ENABLE");
        }

        public override void OnGameInstanceIsReady()
        {
            base.OnGameInstanceIsReady();
            QRDebug.DebugInfo(Constants.DebuggingChannels.LifeCycle, "TEST -> ON_GAME_INSTANCE_IS_READY");
        }
    }
}
