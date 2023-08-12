namespace QRCode.Engine.Core.GameLevel
{
    using Toolbox;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Events;

    public class GameLevelLoaderComponent : SerializedMonoBehaviour
    {
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [SerializeField] private GameLevelLoader gameLevelLoader = new GameLevelLoader();

        [TitleGroup(Constants.InspectorGroups.GameEvents)] 
        [SerializeField] private UnityEvent m_onBeforeLoadLevel = new UnityEvent();
        [TitleGroup(Constants.InspectorGroups.GameEvents)]
        [SerializeField] private UnityEvent m_onAfterLoadLevel = new UnityEvent();
        
        [ButtonGroup(Constants.InspectorGroups.Debugging)]
        public async void ChangeLevel()
        {
            m_onBeforeLoadLevel.Invoke();
            await gameLevelLoader.ChangeLevel();
            m_onAfterLoadLevel.Invoke(); 
        }
    }
}
