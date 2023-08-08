namespace QRCode.Engine.Core.GameState
{
    using Framework.Singleton;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class EasyGameStateSetup : MonoBehaviourSingleton<EasyGameStateSetup>
    {
        [InfoBox("Select a Game State to start the game at a specific point.", InfoMessageType.Info)] 
        [SerializeField] private string m_startAtGameStateName;
        
        public void JumpToGameState()
        {
            GameStateManager.Instance.JumpToGameState(m_startAtGameStateName);
        }
    }
}