namespace QRCode.Engine.Core.UI
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using QRCode.Engine.Core.Manager;
	using QRCode.Engine.Core.Tags;
	using QRCode.Engine.Debugging;

	public class CanvasManager : GenericManagerBase<CanvasManager>
	{
		private Dictionary<CanvasEnum, UICanvas> _allCanvas = null;

		protected override Task InitAsync(CancellationToken cancellationToken)
		{
			_allCanvas = new Dictionary<CanvasEnum, UICanvas>();
			
			return Task.CompletedTask;
		}

		public UICanvas GetCanvas(CanvasEnum canvasEnum)
		{
			return _allCanvas[canvasEnum];
		}
		
		public void RegisterUICanvas(CanvasEnum canvasEnum, UICanvas uiCanvas)
		{
			if (_allCanvas == null)
			{
				QRLogger.DebugError<CoreTags.UI>($"{_allCanvas} is null, something went wrong in execution order initialization.");
				return;
			}
			
			_allCanvas.Add(canvasEnum, uiCanvas);
		}

		public void UnregisterUICanvas(CanvasEnum canvasEnum)
		{
			_allCanvas.Remove(canvasEnum);
		}
	}
}