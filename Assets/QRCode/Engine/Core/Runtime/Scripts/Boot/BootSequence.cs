namespace QRCode.Engine.Core.Boot
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Debugging;
    using QRCode.Engine.Core.Tags;

    /// <summary>
    /// This class is used to play in sequence asynchronous task implemented by <see cref="IBootStep"/>.
    /// </summary>
    public class BootSequence
    {
        /// <summary>
        /// Play in sequence asynchronous task implemented by <see cref="IBootStep"/>.
        /// </summary>
        /// <param name="bootSteps"></param>
        /// <returns></returns>
        public async Task<BootResult> PlayBootSequence(List<IBootStep> bootSteps)
        {
            for (int i = 0; i < bootSteps.Count; i++)
            {
                var result = await bootSteps[i].ExecuteBootStep();

                if (result == BootResult.Fail)
                {
                    QRLogger.DebugFatal<CoreTags.Boot>($"BootStep {bootSteps.GetType()} result with Fail status.");
                    return BootResult.Fail;
                }
            }

            return BootResult.Success;
        }
    }
}
