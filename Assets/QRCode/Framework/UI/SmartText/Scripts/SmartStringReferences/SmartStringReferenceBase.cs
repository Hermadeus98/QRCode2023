namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public abstract class SmartStringReferenceBase<T> : SerializedScriptableObject
    {
        [SerializeField] private T m_initialValue;
        [ReadOnly] [SerializeField] private T m_value;
        public virtual T Value
        {
            get => m_value;
        }

        protected override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            m_value = m_initialValue;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                m_value = m_initialValue;
            }
        }
        #endif
    }
}
