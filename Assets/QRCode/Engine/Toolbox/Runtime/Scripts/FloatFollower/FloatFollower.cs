namespace QRCode.Engine.Toolbox
{
    using System;
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public class FloatFollower
    {
        [SerializeField] private FollowerUpdateMode m_followerUpdateMode;
        [SerializeField] private Vector2 m_bounds = new Vector2(-1f, 1f);
        [SerializeField][ShowIf("@this.m_followerUpdateMode == FollowerUpdateMode.Update")] private float m_speed = .1f;
        [SerializeField][ShowIf("@this.m_followerUpdateMode == FollowerUpdateMode.Tween")] private TweenParameters<float> m_tweenParameters;

        //CurrentValue tend to be equal at this value;
        private float m_targetValue;
        private float m_currentValue;
        private Tween m_tween;

        private const float THRESHOLD = .01f;

        public void SetValue(float newValue, bool force = false)
        {
            if (force)
            {
                m_currentValue = newValue;
                m_currentValue = Mathf.Clamp(m_targetValue, m_bounds.x, m_bounds.y);
                return;
            }

            if (m_followerUpdateMode == FollowerUpdateMode.Tween)
            {
                m_tweenParameters.ToValue = newValue;
                m_tween?.Kill();
                m_tween = DOTween.To(
                        () => m_currentValue,
                        (x) => m_currentValue = x,
                        newValue,
                        m_tweenParameters.Duration)
                    .SetEase(m_tweenParameters.Ease)
                    .SetDelay(m_tweenParameters.Delay);
                return;
            }
            
            m_targetValue = newValue;
            m_targetValue = Mathf.Clamp(m_targetValue, m_bounds.x, m_bounds.y);
        }

        public float GetValue()
        {
            return m_currentValue;
        }

        public void Update()
        {
            if (m_followerUpdateMode == FollowerUpdateMode.Update)
            {
                if (m_currentValue < m_targetValue)
                {
                    m_currentValue += m_speed * Time.deltaTime;
                }
                else if (m_currentValue > m_targetValue)
                {
                    m_currentValue -= m_speed * Time.deltaTime;
                }

                if (Mathf.Abs(m_targetValue - m_currentValue) < THRESHOLD)
                {
                    m_currentValue = m_targetValue;
                }
            }
        }
    }
    
    public enum FollowerUpdateMode
    {
        Update = 0,
        Tween = 1,
    }
}
