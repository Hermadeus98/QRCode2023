namespace QRCode.Engine.Core.UI
{
    using QRCode.Engine.Toolbox.Optimization;
    using Sirenix.OdinInspector;
    using UnityEngine;
    
    [RequireComponent(typeof(CanvasGroup))]
    public class UIElement : SerializedMonoBehaviour, IDeletable
    {
        private RectTransform _rectTransform = null;

        protected RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = transform as RectTransform;
                }

                return _rectTransform;
            }
        }

        private CanvasGroup m_canvasGroup = null;
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (m_canvasGroup == null)
                {
                    m_canvasGroup = GetComponent<CanvasGroup>();
                }
                return m_canvasGroup;
            }
        }

        protected virtual void Start()
        {
            Initialize();
        }

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnDisable()
        {
            
        }

        public virtual void Initialize()
        {
            
        }

        public virtual void RefreshUI()
        {
            
        }
        
        public virtual void UpdateCosmetics()
        {
            
        }

        protected virtual void OnShow()
        {
            
        }

        protected virtual void OnHide()
        {
            
        }

        protected void OnDestroy()
        {
            Delete();
        }

        public virtual void Delete()
        {
            
        }
    }
}
