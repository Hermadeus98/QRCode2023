namespace QRCode.Engine.Core.UI
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using QRCode.Engine.Core.Manager;

	public class CanvasManager : GenericManagerBase<CanvasManager>
	{
		private Dictionary<CanvasEnum, UICanvas> _allCanvas = null;

		protected override Task InitAsync(CancellationToken cancellationToken)
		{
			_allCanvas = new Dictionary<CanvasEnum, UICanvas>();
			
			return Task.CompletedTask;
		}

		public override void Delete()
		{
			base.Delete();
		}

		public UICanvas GetCanvas(CanvasEnum canvasEnum)
		{
			return _allCanvas[canvasEnum];
		}
		
		public void RegisterUICanvas(CanvasEnum canvasEnum, UICanvas uiCanvas)
		{
			_allCanvas.Add(canvasEnum, uiCanvas);
		}

		public void UnregisterUICanvas(CanvasEnum canvasEnum)
		{
			_allCanvas.Remove(canvasEnum);
		}
	}
}