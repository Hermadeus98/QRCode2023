namespace QRCode.Engine.Core.Tests
{
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Debugging;

    public class GameplayComponentExample : GameplayComponent
    {
        private void Awake()
        {
            QRLogger.DebugInfo<CoreTags.LifeCycle>("GAMEPLAY COMPONENT TEST -> AWAKE");
        }

        private void Start()
        {
            QRLogger.DebugInfo<CoreTags.LifeCycle>("GAMEPLAY COMPONENT TEST -> START");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            QRLogger.DebugInfo<CoreTags.LifeCycle>("GAMEPLAY COMPONENT TEST -> ON_ENABLE");
        }

        public override void OnGameInstanceIsReady()
        {
            base.OnGameInstanceIsReady();
            QRLogger.DebugInfo<CoreTags.LifeCycle>("GAMEPLAY COMPONENT TEST -> ON_GAME_INSTANCE_IS_READY");
        }

        public override void OnLevelLoaded()
        {
            base.OnLevelLoaded();
            QRLogger.DebugInfo<CoreTags.LifeCycle>("GAMEPLAY COMPONENT TEST -> ON_LEVEL_LOADED");
        }
        
        public override void OnLevelUnloaded()
        {
            base.OnLevelLoaded();
            QRLogger.DebugInfo<CoreTags.LifeCycle>("GAMEPLAY COMPONENT TEST -> ON_LEVEL_UNLOADED");
        }
    }
}
