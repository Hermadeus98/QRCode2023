namespace QRCode.Framework
{
    using System.Threading;
    using System.Threading.Tasks;
    using Engine.Core;
    using Engine.Core.GameState;
    using UnityEngine;

    public class GameStateGameInitialization : GameStateBase
    {
        private CancellationTokenSource cancellationTokenSource = null;
        
        protected override async void OnEnter(Animator animator)
        {
            cancellationTokenSource = new CancellationTokenSource();
            
            while (GameInstance.Instance.IsReady == false)
            {
                if (cancellationTokenSource.Token.IsCancellationRequested == true)
                {
                    return;
                }
                
                await Task.Yield();
            }
            
            GameStateManager.Instance.SetBool(GameStateManager.IsInitHash, true);
        }

        protected override void OnUpdate(Animator animator)
        {
            
        }

        protected override void OnExit(Animator animator)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }
}
