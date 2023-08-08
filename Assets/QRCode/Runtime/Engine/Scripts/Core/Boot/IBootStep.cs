namespace QRCode.Engine.Core
{
    using System.Threading.Tasks;

    public interface IBootStep
    {
        public Task<BootResult> ExecuteBootStep();
    }

    public enum BootResult
    {
        Success,
        Fail
    }
}
