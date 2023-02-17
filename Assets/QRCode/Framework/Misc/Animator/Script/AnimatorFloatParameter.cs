namespace QRCode.Framework
{
    using System;
    using UnityEngine;

    [Serializable]
    public class AnimatorFloatParameter : AnimatorParameter<float>
    {
        [SerializeField] private FloatFollower m_floatFollower = new FloatFollower();

        public override void Initialize()
        {
            base.Initialize();
            Updater.Instance.RegisterEvent(UpdateModeEnum.Update, LerpFloat);
        }

        public override float GetValue()
        {
            return m_animator.GetFloat(m_parameterHash);
        }

        public override void SetValue(float value)
        {
            m_floatFollower.SetValue(value);
        }

        private void LerpFloat()
        {
            m_floatFollower.Update();
            m_animator.SetFloat(m_parameterHash, m_floatFollower.GetValue());
        }

        public override void Dispose()
        {
            base.Dispose();
            Updater.Instance.UnregisterEvent(UpdateModeEnum.Update, LerpFloat);
        }
    }
}
