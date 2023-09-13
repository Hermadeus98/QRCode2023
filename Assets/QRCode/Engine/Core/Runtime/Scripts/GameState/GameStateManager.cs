namespace QRCode.Engine.Core.GameState
{
    using System.Threading;
    using System.Threading.Tasks;
    using QRCode.Engine.Core.Manager;
    using QRCode.Engine.Toolbox;
    using QRCode.Engine.Toolbox.Optimization;
    using Sirenix.OdinInspector;
    using UnityEngine;

    /// <summary>
    /// This class manage all the game states of the application.
    /// </summary>
    public class GameStateManager : GenericManagerBase<GameStateManager>, IDeletable
    {
        #region Fields
        #region Serialized
        [TitleGroup(Constants.InspectorGroups.References)] 
        [Tooltip("The reference of the animator that controls game states.")]
        [SerializeField] private Animator m_gameStateAnimator = null;
        #endregion Serialized

        #region Statics
        public static readonly int IsInitHash = Animator.StringToHash("IsInit");
        #endregion Statics
        #endregion Fields

        #region Methods
        #region Lifecycle
        protected override Task InitAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override void Delete()
        {
            m_gameStateAnimator = null;
            base.Delete();
        }
        #endregion Lifecycle

        #region Publics
        public void SetBool(int id, bool value)
        {
            m_gameStateAnimator.SetBool(id, value);
        }

        public void SetTrigger(int id)
        {
            m_gameStateAnimator.SetTrigger(id);
        }

        public void SetInt(int id, int value)
        {
            m_gameStateAnimator.SetInteger(id, value);
        }

        public void SetFloat(int id, float value)
        {
            m_gameStateAnimator.SetFloat(id, value);
        }
        
        public void JumpToGameState(string gameStateName)
        {
            m_gameStateAnimator.Play(gameStateName);
        }
        #endregion Publics
        #endregion Methods
    }
}
