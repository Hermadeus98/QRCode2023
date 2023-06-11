namespace QRCode.Framework
{
    using System;
    using UnityEngine;

    public abstract class AnimatorParameter<T>
    {
        [SerializeField] protected string m_parameterName;
        [SerializeField] protected Animator m_animator;
        protected int m_parameterHash;

        public virtual void Initialize()
        {
            m_parameterHash = Animator.StringToHash(m_parameterName);
        }

        public abstract T GetValue();
        public abstract void SetValue(T value);
    }
}
