namespace QRCode.Engine.Core.Boot
{
	using System.Threading.Tasks;
	using QRCode.Engine.Core.Tags;
	using QRCode.Engine.Debugging;
	using QRCode.Engine.Toolbox.Optimization;

	/// <summary>
	/// This class should contains the boot of the game, like splash screens, manager initialization awaiter, etc.
	/// </summary>
	public class Boot : IDeletable
	{
		private static readonly IBootStep[] _bootSteps = new IBootStep[]
		{
			new ManagerBoot(),
		};
		
		private BootSequence _bootSequence = null;
		
		public async Task Execute()
		{
			_bootSequence = new BootSequence(_bootSteps);
			var result = await _bootSequence.PlayBootSequence();

			if (result == BootResult.Fail)
			{
				QRLogger.DebugFatal<CoreTags.Boot>($"Application boot finish with an error.");
			}
			
			QRLogger.Debug<CoreTags.Boot>($"Application boot finish with success.");	
		}

		public void Delete()
		{
			if (_bootSequence != null)
			{
				_bootSequence.Delete();
			}
		}
	}
}