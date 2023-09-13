namespace QRCode.Engine.Core.GameState
{
    using Debugging;
    using QRCode.Engine.Core.Tags;
    using UnityEngine;
    
    /// <summary>
    /// Defines the game flow, contains the base class of the current game mode.
    /// Game States should defines which game views are enable and available, and defines which input maps are enabled. 
    /// </summary>
    public abstract class GameStateBase : StateMachineBehaviour
    {
        [SerializeField] private string m_gameStateName = "Game State with no name...";
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            QRLogger.DebugInfo<CoreTags.GameStates>($"Enter in : {m_gameStateName}");
            base.OnStateEnter(animator, stateInfo, layerIndex);
            OnEnter(animator);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            QRLogger.DebugInfo<CoreTags.GameStates>($"Exit : {m_gameStateName}");

            base.OnStateExit(animator, stateInfo, layerIndex);
            OnExit(animator);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            OnUpdate(animator);
        }

        protected abstract void OnEnter(Animator animator);

        protected abstract void OnUpdate(Animator animator);
        
        protected abstract void OnExit(Animator animator);
    }
}
