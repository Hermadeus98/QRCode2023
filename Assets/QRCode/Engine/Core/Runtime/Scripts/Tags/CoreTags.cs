namespace QRCode.Engine.Core.Tags
{
	using QRCode.Engine.Debugging;

	/// <summary>
	/// All the tags of the Core part of Engine.
	/// </summary>
	public class CoreTags : LoggerTag
	{
		/// <summary>
		/// The tag for the lifecycle logs.
		/// </summary>
		public class LifeCycle : ILoggerTag { }
		
		/// <summary>
		/// The tag for the user settings logs.
		/// </summary>
		public class UserSettings : ILoggerTag { }
		
		/// <summary>
		/// The tag for the save and load logs.
		/// </summary>
		public class Save : ILoggerTag { }
		
		/// <summary>
		/// The tag for the audio logs.
		/// </summary>
		public class Audio : ILoggerTag { }
		
		/// <summary>
		/// The tag for the boot logs.
		/// </summary>
		public class Boot : ILoggerTag { }
		
		/// <summary>
		/// The tag for the game instance logs.
		/// </summary>
		public class GameInstance : ILoggerTag { }
		
		/// <summary>
		/// The tag for the game levels logs.
		/// </summary>
		public class GameLevels : ILoggerTag { }
		
		/// <summary>
		/// The tag for the game modes logs.
		/// </summary>
		public class GameModes : ILoggerTag { }
		
		/// <summary>
		/// The tag for the game states logs.
		/// </summary>
		public class GameStates : ILoggerTag { }
		
		/// <summary>
		/// The tag for the inputs logs.
		/// </summary>
		public class Inputs : ILoggerTag { }
		
		/// <summary>
		/// The tag for the remote configs logs.
		/// </summary>
		public class RemoteConfigs : ILoggerTag { }
		
		/// <summary>
		/// The tag for the scene management logs.
		/// </summary>
		public class SceneManagement : ILoggerTag { }
	}
}