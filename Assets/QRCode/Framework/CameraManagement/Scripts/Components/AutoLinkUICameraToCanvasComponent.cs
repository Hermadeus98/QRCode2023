namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    [ExecuteInEditMode]
    public class AutoLinkUICameraToCanvasComponent : MonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.References)] 
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
                var uiCamera = UICamera.Instance;
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
