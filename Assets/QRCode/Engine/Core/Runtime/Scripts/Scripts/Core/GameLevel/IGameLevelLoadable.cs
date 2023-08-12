namespace QRCode.Engine.Core.GameLevel
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Localization;

    public interface IGameLevelLoadable
    {
        public GameLevelLoadProgressionInfos GameLevelLoadProgressionInfos { get; set; }
        public Task Load(CancellationToken cancellationToken, Action onLoading, IProgress<float> progress);
    }

    public struct GameLevelLoadProgressionInfos
    {
        [SerializeField] private LocalizedString m_loadingProgressionDescription;
        [SerializeField][ReadOnly] private float m_loadingProgressPercent;

        public float LoadingProgressPercent
        {
            get => m_loadingProgressPercent;
            set => m_loadingProgressPercent = value;
        }
        public LocalizedString ProgressionDescription
        {
            get => m_loadingProgressionDescription;
            set => m_loadingProgressionDescription = value;
        }
    }
}
