namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    
    public class UIElement : SerializedMonoBehaviour
    {
        public RectTransform RectTransform
        {
            get => transform as RectTransform;
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

        protected virtual void OnDestroy()
        {
            
        }
    }
}
