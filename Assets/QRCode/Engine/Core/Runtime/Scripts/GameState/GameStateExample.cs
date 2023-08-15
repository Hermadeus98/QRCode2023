namespace QRCode.Engine.Core.GameState
{
    using GameLevels;
    using UnityEngine;

    public class GameStateExample : GameStateBase
    {
        [SerializeField] private GameLevelLoader gameLevelLoader = null;
        
        protected override async void OnEnter(Animator animator)
        {
            await gameLevelLoader.ChangeLevel();
        }

        protected override void OnUpdate(Animator animator)
        {
        }

        protected override void OnExit(Animator animator)
        {
        }
    }
}