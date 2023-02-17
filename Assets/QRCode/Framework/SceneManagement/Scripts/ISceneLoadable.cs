namespace QRCode.Framework
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public interface ISceneLoadable
    {
        public SceneLoadableProgressionInfos SceneLoadableProgressionInfos { get; set; }
        public Task Load(CancellationToken cancellationToken, Action onLoading, IProgress<float> progress);
    }

    public struct SceneLoadableProgressionInfos
    {
        [SerializeField] private string m_loadingProgressionDescription;
        [SerializeField][ReadOnly] private float m_loadingProgressPercent;

        public float LoadingProgressPercent
        {
            get => m_loadingProgressPercent;
            set => m_loadingProgressPercent = value;
        }
        public string ProgressionDescription
        {
            get => m_loadingProgressionDescription;
            set => m_loadingProgressionDescription = value;
        }
    }
}
