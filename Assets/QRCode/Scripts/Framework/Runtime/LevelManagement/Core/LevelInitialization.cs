namespace QRCode.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Game;
    using SceneManagement;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class LevelInitialization : SerializedMonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.Settings)]
        [SerializeField] private List<ISceneLoadable> m_sceneLoadables = new List<ISceneLoadable>();

        [TitleGroup(K.InspectorGroups.Settings)] [SerializeField]
        private DB_LoadingScreenEnum m_loadingScreenEnum = DB_LoadingScreenEnum.LoadingScreenBase;

        [TitleGroup(K.InspectorGroups.Debugging)]
        [ShowInInspector][ReadOnly] private bool m_initializedBySceneManager = false;

        private bool m_levelIsLoaded = false;
        private CancellationTokenSource m_cancellationTokenSource = null;
        private ILoadingScreen m_loadingScreen = null;
        private SceneLoadingInfo m_sceneLoadingInfo;

        public static LevelInitialization Current = null;
        
        private void Awake()
        {
            Current = this;
            Initialize();
        }

        private async void Initialize()
        {
            while (Bootstrap.IsInit() == false)
            {
                await Task.Yield();
            }
            
            if (ServiceLocator.Current.Get<ILevelLoadingManagementService>().IsLoading())
            {
                return;
            }
            
            m_levelIsLoaded = false;

            m_cancellationTokenSource = new CancellationTokenSource();
                
            m_loadingScreen = await UI.GetLoadingScreen(m_loadingScreenEnum);
            await m_loadingScreen.Show();

            m_sceneLoadingInfo = new SceneLoadingInfo();

            var progression = new Progress<SceneLoadableProgressionInfos>(value =>
            {
                m_sceneLoadingInfo.GlobalProgress = value.LoadingProgressPercent;
                m_sceneLoadingInfo.ProgressDescription = value.ProgressionDescription.GetLocalizedString();
            });

            if (m_initializedBySceneManager == true)
            {
                return;
            }
            
            OnLoadingScenes();
            
            await LoadLevel(m_cancellationTokenSource.Token, progression);

            await m_loadingScreen.Hide();
        }

        private async void OnLoadingScenes()
        {
            while (m_levelIsLoaded == false && m_cancellationTokenSource.IsCancellationRequested == false)
            {
                m_loadingScreen.Progress(m_sceneLoadingInfo);
                await Task.Yield();
            }
        }

        public void ForceInitializationFromSceneManager()
        {
            m_initializedBySceneManager = true;
        }

        public void UnloadLevel()
        {
            GameInstance.Instance.OnLevelUnloaded();    
        }
        
        public async Task LoadLevel(CancellationToken cancellationToken, IProgress<SceneLoadableProgressionInfos> progress)
        {
            var sceneLoadableProgressionInfos = new SceneLoadableProgressionInfos();
            
            var currentSceneLoadableProgression = 0f;
            var progression = new Progress<float>(value =>
            {
                currentSceneLoadableProgression = value;
            });
            
            var sceneLoadableCount = m_sceneLoadables.Count;
            for (var i = 0; i < sceneLoadableCount; i++)
            {
                var index = i;
                var onLoading = new Action(() =>
                {
                    sceneLoadableProgressionInfos.LoadingProgressPercent = (index + currentSceneLoadableProgression) / sceneLoadableCount;
                    sceneLoadableProgressionInfos.ProgressionDescription = m_sceneLoadables[i].SceneLoadableProgressionInfos.ProgressionDescription;
                    progress.Report(sceneLoadableProgressionInfos);
                });
                
                var loading = m_sceneLoadables[i].Load(cancellationToken, onLoading, progression);
                await loading;

                sceneLoadableProgressionInfos.LoadingProgressPercent = (i + 1f) / sceneLoadableCount;
                progress.Report(sceneLoadableProgressionInfos);
            }

            sceneLoadableProgressionInfos.LoadingProgressPercent = 1f;
            progress.Report(sceneLoadableProgressionInfos);
            
            GameInstance.Instance.OnLevelLoaded();

            m_levelIsLoaded = true;
        }
    }
}
