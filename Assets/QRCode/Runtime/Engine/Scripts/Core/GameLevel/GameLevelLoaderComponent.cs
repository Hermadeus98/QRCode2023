namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Events;

    public class GameLevelLoaderComponent : SerializedMonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.Settings)]
        [SerializeField] private GameLevelLoader gameLevelLoader = new GameLevelLoader();

        [TitleGroup(K.InspectorGroups.GameEvents)] 
        [SerializeField] private UnityEvent m_onBeforeLoadLevel = new UnityEvent();
        [TitleGroup(K.InspectorGroups.GameEvents)]
        [SerializeField] private UnityEvent m_onAfterLoadLevel = new UnityEvent();
        
        [ButtonGroup(K.InspectorGroups.Debugging)]
        public async void ChangeLevel()
        {
            m_onBeforeLoadLevel.Invoke();
            await gameLevelLoader.ChangeLevel();
            m_onAfterLoadLevel.Invoke(); 
        }
    }
}
