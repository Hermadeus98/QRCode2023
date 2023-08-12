namespace QRCode.Engine.Core.Boot
{
    using System.Threading.Tasks;

    /// <summary>
    /// Implement this interface when the code need to be execute in a <see cref="BootSequence"/>.
    /// </summary>
    public interface IBootStep
    {
        /// <summary>
        /// The step executed in the <see cref="BootSequence"/>.
        /// </summary>
        /// <returns></returns>
        public Task<BootResult> ExecuteBootStep();
    }

    /// <summary>
    /// The result of the <see cref="IBootStep"/> :
    /// - <exception cref="Success"></exception> : the step is a success, the sequence will continue.
    /// - <see cref="Fail"/> : the step handle an error, the sequence will be stopped.
    /// </summary>
    public enum BootResult
    {
        Success,
        Fail
    }
}
