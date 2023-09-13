namespace QRCode.Engine.Core.UI
{
    using UnityEngine;

    public class UICanvas : MonoBehaviour
    {
        [SerializeField] private CanvasEnum m_canvasEnum = CanvasEnum.Undifined;
        
        private void OnEnable()
        {
            CanvasManager.Instance.RegisterUICanvas(m_canvasEnum, this);
        }

        private void OnDisable()
        {
            CanvasManager.Instance.UnregisterUICanvas(m_canvasEnum);
        }
    }

    public enum CanvasEnum
    {
        Undifined = 0,
        GameCanvas = 1,
        LoadingScreenCanvas = 2,
        SubtitleCanvas = 3,
        Save = 4,
        Cursor = 5,
        Debug = 6,
    }
}
