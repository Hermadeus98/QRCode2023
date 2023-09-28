namespace QRCode.Engine.Core.Boot
{
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// This boot will check that all managers are initialized correctly.
	/// </summary>
	public class ManagerBoot : IBootStep
	{
		public void Delete() { }

		public async Task<BootResult> ExecuteBootStep(CancellationToken cancellationToken)
		{
			ManagerBootComponent managerBootComponent = ManagerBootComponent.Instance;
			return await managerBootComponent.ExecuteBootStep(cancellationToken);
		}
	}
}