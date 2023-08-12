namespace QRCode.Engine.Core.Camera
{
    using Toolbox;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [ExecuteInEditMode]
    public class AutoLinkUICameraToCanvasComponent : MonoBehaviour
    {
        [TitleGroup(Constants.InspectorGroups.References)] 
        [SerializeField] private Canvas m_canvas = null;
        
        private void Start()
        {
            LinkUICamera();
        }

        private void Update()
        {
#if UNITY_EDITOR
            LinkUICamera();
#endif
        }

        private void LinkUICamera()
        {
            if (m_canvas.worldCamera == null)
            {
                UICamera uiCamera = null;
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    uiCamera = UICamera.Instance;
                }
                else
                {
                    uiCamera = FindObjectOfType<UICamera>();
                }
#else
                uiCamera = UICamera.Instance;
#endif
                if (uiCamera == null)
                {
                    return;
                }
                
                m_canvas.worldCamera = uiCamera.Camera;
            }
        }

        private void OnValidate()
        {
            if (m_canvas == null)
            {
                m_canvas = GetComponent<Canvas>();
            }
        }
    }
}
