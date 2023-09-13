namespace QRCode.Engine.Game.Tags
{
	using QRCode.Engine.Debugging;

	/// <summary>
	/// All the tags of the Game part of Engine.
	/// </summary>
	public class GameTags : LoggerTag
	{
		/// <summary>
		/// The tag used for input hints logs.
		/// </summary>
		public class InputHints : ILoggerTag { }
		
		/// <summary>
		/// The tag used for loading screen logs.
		/// </summary>
		public class LoadingScreen : ILoggerTag { }
		
		/// <summary>
		/// The tag used for subtitles logs.
		/// </summary>
		public class Subtitles : ILoggerTag { }
	}
}