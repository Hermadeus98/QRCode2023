namespace QRCode.Engine.Core.GameLevel.Tests
{
    using UnityEngine;
    
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Engine.Core.GameLevels;

    public class GameLevelModuleLoadableTest : GameLevelModuleBase<GameLevelModuleLoadableTestData>, IGameLevelModule
    {
        public GameLevelModuleLoadableTest(GameLevelModuleLoadableTestData moduleData) : base(moduleData)
        {
        }

        public override async Task Load(CancellationToken cancellationToken, Action onLoading, IProgress<float> progress)
        {
            var elapsedTime = 0f;
            
            while (elapsedTime < m_moduleData.WaitDuration)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                elapsedTime += Time.deltaTime;
                if (elapsedTime > m_moduleData.WaitDuration) elapsedTime = m_moduleData.WaitDuration;

                onLoading?.Invoke();

                var progressionPercent = elapsedTime / m_moduleData.WaitDuration;
                
                var sceneLoadableProgressionInfos = GameLevelLoadingInfo;
                sceneLoadableProgressionInfos.LoadingProgressPercent = progressionPercent;
                
                progress.Report(progressionPercent);
                
                await Task.Yield();
            }
            
            progress.Report(1f);
        }

        public override void Delete()
        {
            
        }
    }
}
