namespace QRCode.Engine.Core.GameMode
{
    using QRCode.Engine.Toolbox.Optimization;

    public abstract class AGameModeModule : IDeletable
    {
        public abstract void OnConstruct(AGameMode aGameMode);
        
        public abstract void Initialize(AGameMode aGameMode);
        
        public virtual void Delete()
        {
            
        }
    }
}
