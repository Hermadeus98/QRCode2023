namespace QRCode.Engine.Core.GameState
{
    using System.Threading.Tasks;
    using Engine.Core.Managers;
    using Framework;
    using Framework.Singleton;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class GameStateManager : MonoBehaviourSingleton<GameStateManager>, IManager
    {
        [TitleGroup(K.InspectorGroups.References)] 
        [SerializeField] private Animator m_gameStateAnimator = null;

        public static readonly int IsInitHash = Animator.StringToHash("IsInit");

        public Task InitAsync()
        {
            return Task.CompletedTask;
        }

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
    }
}
