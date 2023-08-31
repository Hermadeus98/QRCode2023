namespace QRCode.Engine.Core.Tests
{
    using Debugging;
    using Constants = Toolbox.Constants;

    public class GameplayComponentExample : GameplayComponent
    {
        private void Awake()
        {
            QRDebug.DebugInfo(Constants.DebuggingChannels.LifeCycle, "GAMEPLAY COMPONENT TEST -> AWAKE");
        }

        private void Start()
        {
            QRDebug.DebugInfo(Constants.DebuggingChannels.LifeCycle, "GAMEPLAY COMPONENT TEST -> START");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            QRDebug.DebugInfo(Constants.DebuggingChannels.LifeCycle, "GAMEPLAY COMPONENT TEST -> ON_ENABLE");
        }

        public override void OnGameInstanceIsReady()
        {
            base.OnGameInstanceIsReady();
            QRDebug.DebugInfo(Constants.DebuggingChannels.LifeCycle, "GAMEPLAY COMPONENT TEST -> ON_GAME_INSTANCE_IS_READY");
        }

        public override void OnLevelLoaded()
        {
            base.OnLevelLoaded();
            QRDebug.DebugInfo(Constants.DebuggingChannels.LifeCycle, "GAMEPLAY COMPONENT TEST -> ON_LEVEL_LOADED");
        }
        
        public override void OnLevelUnloaded()
        {
            base.OnLevelLoaded();
            QRDebug.DebugInfo(Constants.DebuggingChannels.LifeCycle, "GAMEPLAY COMPONENT TEST -> ON_LEVEL_UNLOADED");
        }
    }
}
