namespace QRCode.Engine.Debugging
{
	public interface ILoggerTag { }
	
	public class LoggerTag
	{
		/// <summary>
		/// The tags for the debugger.
		/// </summary>
		public class Debugger: ILoggerTag { }
	}
}