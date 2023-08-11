namespace QRCode.Engine.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Framework.Debugging;

    public class BootSequence
    {
        public async Task<BootResult> PlayBootSequence(List<IBootStep> bootSteps)
        {
            for (int i = 0; i < bootSteps.Count; i++)
            {
                var result = await bootSteps[i].ExecuteBootStep();

                if (result == BootResult.Fail)
                {
                    QRDebug.DebugFatal(Constants.EngineConstants.EngineLogChannels.EngineChannel, $"BootStep {bootSteps.GetType().ToString()} result with Fail status.");
                    return BootResult.Fail;
                }
            }

            return BootResult.Success;
        }
    }
}
