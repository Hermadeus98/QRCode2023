namespace QRCode.Gameplay.Pooling
{
    public interface IPoolObject
    {
        public bool IsAvailable { get; set; }
        public void OnPool();
        public void OnPush();
    }
}
