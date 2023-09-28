namespace QRCode.Engine.Core.Boot
{
    using System.Threading;
    using System.Threading.Tasks;
    using Debugging;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Toolbox.Optimization;

    /// <summary>
    /// This class is used to play in sequence asynchronous task implemented by <see cref="IBootStep"/>.
    /// </summary>
    public class BootSequence : IDeletable
    {
        private IBootStep[] _bootSteps = null;
        private CancellationTokenSource _cancellationTokenSource = null;

        public BootSequence(IBootStep[] bootSteps)
        {
            _bootSteps = bootSteps;
            _cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Delete()
        {
            if (_bootSteps != null)
            {
                for (int i = 0; i < _bootSteps.Length; i++)
                {
                    _bootSteps[i].Delete();
                }
                
                _bootSteps = null;
            }

            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }
        
        /// <summary>
        /// Play in sequence asynchronous task implemented by <see cref="IBootStep"/>.
        /// </summary>
        public async Task<BootResult> PlayBootSequence()
        {
            for (int i = 0; i < _bootSteps.Length; i++)
            {
                var result = await _bootSteps[i].ExecuteBootStep(_cancellationTokenSource.Token);

                if (result == BootResult.Fail)
                {
                    QRLogger.DebugFatal<CoreTags.Boot>($"BootStep {_bootSteps.GetType()} result with Fail status.");
                    return BootResult.Fail;
                }
            }

            return BootResult.Success;
        }
    }
}
