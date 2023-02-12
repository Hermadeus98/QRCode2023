namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class AnimatorControllerTest : SerializedMonoBehaviour
    {
        [SerializeField] private AnimatorFloatParameter m_animatorFloatParameter;
        [SerializeField] private float m_value;

        private void Start()
        {
            m_animatorFloatParameter.Initialize();
        }

        [Button]
        private void UpdateValue()
        {
            m_animatorFloatParameter.SetValue(m_value);
        }
    }
}
