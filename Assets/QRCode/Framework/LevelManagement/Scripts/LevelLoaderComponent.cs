namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Events;

    public class LevelLoaderComponent : SerializedMonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private bool m_playOnStart = false;
        [TitleGroup(K.InspectorGroups.Settings)]
        [SerializeField] private LevelLoader levelLoader = new LevelLoader();

        [TitleGroup(K.InspectorGroups.GameEvents)] 
        [SerializeField] private UnityEvent m_onBeforeLoadLevel = new UnityEvent();
        [TitleGroup(K.InspectorGroups.GameEvents)]
        [SerializeField] private UnityEvent m_onAfterLoadLevel = new UnityEvent();

        public LevelLoader LevelLoader => levelLoader;

        private void Start()
        {
            if (m_playOnStart)
            {
                LoadScenes();
            }
        }

        [ButtonGroup(K.InspectorGroups.Debugging)]
        public async void ChangeLevel()
        {
            m_onBeforeLoadLevel.Invoke();
            await levelLoader.ChangeLevel();
            m_onAfterLoadLevel.Invoke(); 
        }
        
        [ButtonGroup(K.InspectorGroups.Debugging)]
        public async void LoadScenes()
        {
            m_onBeforeLoadLevel.Invoke();
            await levelLoader.LoadLevel();
            m_onAfterLoadLevel.Invoke();
        }

        [ButtonGroup(K.InspectorGroups.Debugging)]
        public async void UnloadScenes()
        {
            await levelLoader.UnloadLevel();
        }
    }
}
