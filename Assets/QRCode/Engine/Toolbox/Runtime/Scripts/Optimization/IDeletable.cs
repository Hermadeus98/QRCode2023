namespace QRCode.Engine.Toolbox.Optimization
{
    /// <summary>
    /// This interface should be implemented on every destroyable object and clear all references.
    /// /!\ This is not call to delete an object, but to reset all this references.
    /// </summary>
    public interface IDeletable
    {
        /// <summary>
        /// Use this function to clear all reference and free memory.
        /// </summary>
        public void Delete();
    }
}