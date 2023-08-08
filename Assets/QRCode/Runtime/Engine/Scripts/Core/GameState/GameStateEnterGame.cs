namespace QRCode.Framework
{
    using Engine.Core.GameState;
    using UnityEngine;

    public class GameStateEnterGame : GameStateBase
    {
        protected override void OnEnter(Animator animator)
        {
            EasyGameStateSetup.Instance.JumpToGameState();
        }

        protected override void OnUpdate(Animator animator)
        {
        }

        protected override void OnExit(Animator animator)
        {
        }
    }
}
