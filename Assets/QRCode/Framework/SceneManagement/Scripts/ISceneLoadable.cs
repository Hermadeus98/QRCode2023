namespace QRCode.Framework
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Localization;

    public interface ISceneLoadable
    {
        public SceneLoadableProgressionInfos SceneLoadableProgressionInfos { get; set; }
        public Task Load(CancellationToken cancellationToken, Action onLoading, IProgress<float> progress);
    }

    public struct SceneLoadableProgressionInfos
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
