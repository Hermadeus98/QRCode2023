namespace QRCode.Engine.Toolbox.Extensions
{
	using UnityEngine;

	public static class TransformExtensions
	{
		public static Transform[] GetChildren(this Transform transform)
		{
			int childCount = transform.childCount;
			Transform[] transforms = new Transform[childCount];
			
			for (int i = 0; i < childCount; i++)
			{
				transforms[i] = transform.GetChild(i);
			}

			return transforms;
		}
	}
}