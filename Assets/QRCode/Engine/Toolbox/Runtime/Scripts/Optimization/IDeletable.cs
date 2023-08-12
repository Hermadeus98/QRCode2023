namespace QRCode.Engine.Toolbox.Optimization
{
    /// <summary>
    /// This interface should be implemented on every destroyable object and clear all references.
    /// </summary>
    public interface IDeletable
    {
        public void Delete();
    }
}