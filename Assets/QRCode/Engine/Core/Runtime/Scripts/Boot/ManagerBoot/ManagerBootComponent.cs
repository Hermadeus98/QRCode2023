namespace QRCode.Engine.Core.Boot
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using QRCode.Engine.Core.Manager;
	using QRCode.Engine.Toolbox.Extensions;
	using QRCode.Engine.Toolbox.Pattern.Singleton;
	using Sirenix.OdinInspector;
	using UnityEngine;

	/// <summary>
	/// This boot will check that all managers are initialized correctly.
	/// </summary>
	public class ManagerBootComponent : MonoBehaviourSingleton<ManagerBootComponent>, IBootStep
	{
		[ReadOnly] 
		[SerializeField] private List<IManager> _allManagers = null;

		public void Delete()
		{
			if (_allManagers != null)
			{
				_allManagers.Clear();
				_allManagers = null;
			}
			
			Destroy(this);
		}
		
		public async Task<BootResult> ExecuteBootStep(CancellationToken cancellationToken)
		{
			FetchAllManagers();

			// Check if all the manager initialization is finish.
			int managerCount = _allManagers.Count;
			for (int i = 0; i < managerCount; i++)
			{
				while (_allManagers[i].IsInit == false)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						return BootResult.Fail;
					}

					await Task.Yield();
				}
			}

			return BootResult.Success;
		}

		private void FetchAllManagers()
		{
			_allManagers = new List<IManager>();

			Transform[] children = transform.GetChildren();
			int childrenCount = children.Length;
			for (int i = 0; i < childrenCount; i++)
			{
				if(children[i].TryGetComponent(out IManager managerBase))
				{
					_allManagers.Add(managerBase);
				}
			}
		}

#if UNITY_EDITOR
		[Button]
		private void EditorFetchAllManagers()
		{
			FetchAllManagers();
		}
#endif
	}
}