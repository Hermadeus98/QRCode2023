namespace QRCode.Framework
{
    using UnityEngine;

    public class UICanvas : MonoBehaviour
    {
        [SerializeField] private CanvasEnum m_canvasEnum = CanvasEnum.Undifined;
        [SerializeField] private bool m_dontDestroyOnLoad = false;

        private void Start()
        {
            if (m_dontDestroyOnLoad)
            {
                transform.SetParent(null);
                DontDestroyOnLoad(this);
            }
        }

        private void OnEnable()
        {
            UI.GetCanvasDatabase.AddToDatabase(m_canvasEnum, this);
        }

        private void OnDisable()
        {
            UI.GetCanvasDatabase.RemoveOfDatabase(m_canvasEnum);
        }
    }

    public enum CanvasEnum
    {
        Undifined = 0,
        GameCanvas = 1,
        LoadingScreenCanvas = 2,
        SubtitleCanvas = 3,
    }
}
