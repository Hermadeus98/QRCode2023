namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Events;

    public class SceneLoaderComponent : SerializedMonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private bool m_playOnStart = false;
        [TitleGroup(K.InspectorGroups.Settings)]
        [SerializeField] private SceneLoader m_sceneLoader = new SceneLoader();

        [TitleGroup(K.InspectorGroups.GameEvents)] 
        [SerializeField] private UnityEvent m_onBeforeLoad = new UnityEvent();
        [TitleGroup(K.InspectorGroups.GameEvents)]
        [SerializeField] private UnityEvent m_onAfterLoad = new UnityEvent();

        public SceneLoader SceneLoader => m_sceneLoader;
        public UnityEvent OnBeforeLoad => m_onBeforeLoad;
        public UnityEvent OnAfterLoad => m_onAfterLoad;

        private void Start()
        {
            if (m_playOnStart)
            {
                LoadScenes();
            }
        }

        [ButtonGroup(K.InspectorGroups.Debugging)]
        public async void LoadScenes()
        {
            m_onBeforeLoad.Invoke();
            await m_sceneLoader.LoadScenes();
            m_onAfterLoad.Invoke();
        }

        [ButtonGroup(K.InspectorGroups.Debugging)]
        public async void UnloadScenes()
        {
            await m_sceneLoader.UnloadScenes();
        }
    }
}
