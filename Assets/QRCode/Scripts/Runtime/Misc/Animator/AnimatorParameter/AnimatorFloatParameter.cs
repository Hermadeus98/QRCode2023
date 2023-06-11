namespace QRCode.Framework
{
    using System;
    using UnityEngine;

    [Serializable]
    public class AnimatorFloatParameter : AnimatorParameter<float>
    {
        [SerializeField] private FloatFollower m_floatFollower = new FloatFollower();

        public override float GetValue()
        {
            return m_animator.GetFloat(m_parameterHash);
        }

        public override void SetValue(float value)
        {
            m_floatFollower.SetValue(value);
        }

        public void LerpFloat()
        {
            m_floatFollower.Update();
            m_animator.SetFloat(m_parameterHash, m_floatFollower.GetValue());
        }
    }
}
