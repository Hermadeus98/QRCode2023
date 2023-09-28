namespace QRCode.Engine.Toolbox.Pattern.Singleton
{
	using UnityEngine;

	/// <summary>
	/// An unique Instance callable everywhere.
	/// </summary>
	public class Singleton<t_type> where t_type : class, new()
	{
		private static t_type _instance;
		public static t_type Instance
		{
			get
			{
				if (_instance == null)
				{
#if UNITY_EDITOR
					if (Application.isPlaying == false)
						return null;
#endif

					return new t_type();
				}
				return _instance;
			}
		}

		/// <summary>
		/// Check if the singleton have already an instance.
		/// </summary>
		public static bool HasInstance()
		{
			return _instance != null;
		}
	}
}