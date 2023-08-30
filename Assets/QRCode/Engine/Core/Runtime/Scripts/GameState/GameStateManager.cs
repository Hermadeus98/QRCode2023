namespace QRCode.Engine.Core.GameState
{
    using UnityEngine;

    using System.Threading;
    using System.Threading.Tasks;
    
    using Sirenix.OdinInspector;

    using Managers;
    using Toolbox;
    using Toolbox.Pattern.Singleton;

    public class GameStateManager : MonoBehaviourSingleton<GameStateManager>, IManager
    {
        #region Fields
        [TitleGroup(Constants.InspectorGroups.References)] 
        [SerializeField] private Animator m_gameStateAnimator = null;

        public static readonly int IsInitHash = Animator.StringToHash("IsInit");
        #endregion

        #region Methods
        #region Initialization
        public Task InitAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnDestroy()
        {
            Delete();
        }

        public void Delete()
        {
            m_gameStateAnimator = null;
        }
        #endregion

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
        #endregion
        #endregion
    }
}
