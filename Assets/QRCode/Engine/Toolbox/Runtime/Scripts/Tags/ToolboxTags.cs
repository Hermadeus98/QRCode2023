namespace QRCode.Engine.Toolbox.Tags
{
	using QRCode.Engine.Debugging;

	/// <summary>
	/// All the tags of the Toolbox part of Engine.
	/// </summary>
	public class ToolboxTags : LoggerTag
	{
		/// <summary>
		/// The tag for the databases logs.
		/// </summary>
		public class Databases : ILoggerTag { }

		/// <summary>
		/// The tag for the services logs.
		/// </summary>
		public class Services : ILoggerTag { }

		/// <summary>
		/// The tag for the patterns logs.
		/// </summary>
		public class Patterns : ILoggerTag
		{
			/// <summary>
			/// The tag for the patterns FiniteStateMachine logs.
			/// </summary>
			public class FiniteStateMachine : ILoggerTag
			{
			
			}
		}
	}
}