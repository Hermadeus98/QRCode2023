namespace QRCode.Engine.Core.Manager
{
    using System.Threading;
    using System.Threading.Tasks;
    using QRCode.Engine.Toolbox.Optimization;
    using QRCode.Engine.Toolbox.Pattern.Singleton;
    using UnityEngine;

    /// <summary>
    /// All manager should inherit from this class.
    /// </summary>
    public abstract class GenericManagerBase<T> : MonoBehaviourSingleton<T>, IDeletable where T : Component
    {
        #region Fields
        #region Internals
        private bool _isInit = false;
        private CancellationTokenSource _cancellationTokenSource = null;
        #endregion Internals
        #endregion Fields

        #region Properties
        /// <summary>
        /// The cancellation token source used to kill all async tasks.
        /// </summary>
        protected CancellationTokenSource CancellationTokenSource { get { return _cancellationTokenSource; } }
        #endregion Properties

        #region Methods
        #region LifeCycle
        private void Awake()
        {
            InitAsyncInternal();
        }

        private void OnDestroy()
        {
            Delete();
        }
        #endregion LifeCycle

        #region Public Methods
        /// <summary>
        /// This function should be used when another process should wait a manager initialization before continue.
        /// </summary>
        public async Task WaitManagerInitialization(CancellationToken cancellationToken)
        {
            while (_isInit == false)
            {
                if (cancellationToken.IsCancellationRequested == true)
                {
                    return;
                }

                await Task.Yield();
            }
        }
        
        /// <summary>
        /// This method should be used to clear all reference and free memory.
        /// </summary>
        public virtual void Delete()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }
        #endregion Public Methods
        
        #region Protected Methods
        /// <summary>
        /// This method should be used to initialize a manager.
        /// </summary>
        protected abstract Task InitAsync(CancellationToken cancellationToken);
        #endregion Protected Methods

        #region Private Methods
        private async void InitAsyncInternal()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            
            await InitAsync(_cancellationTokenSource.Token);
            _isInit = true;
        }
        #endregion Private Methods
        #endregion Methods
    }
}