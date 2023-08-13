namespace QRCode.Engine.Core.DebugInformation
{
    using TMPro;
    using UnityEngine;

    public class FPSTextComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_text = null;

        private int lastFrameIndex;
        private float[] frameDeltaTimeArray;

        private const int FRAME_DELTA_TIME_ARRAY_SIZE = 50;

        private void Awake()
        {
            frameDeltaTimeArray = new float[FRAME_DELTA_TIME_ARRAY_SIZE];
        }

        private void Update()
        {
            frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
            lastFrameIndex = (lastFrameIndex + 1) % FRAME_DELTA_TIME_ARRAY_SIZE;
            
            m_text.SetText(Mathf.RoundToInt(CalculateFrame()).ToString() + " FPS");
        }

        private void OnDestroy()
        {
            m_text = null;
        }

        private float CalculateFrame()
        {
            var total = 0f;
            foreach (var dt in frameDeltaTimeArray)
            {
                total += dt;
            }

            return FRAME_DELTA_TIME_ARRAY_SIZE / total;
        }
    }
}
