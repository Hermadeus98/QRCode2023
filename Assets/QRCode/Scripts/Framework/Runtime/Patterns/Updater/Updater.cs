namespace QRCode.Framework
{
    using System;
    using Singleton;

    public class Updater : MonoBehaviourSingleton<Updater>
    {
        private Action m_onUpdate;
        private Action m_onFixedUpdate;
        private Action m_onLateUpdate;

        public void RegisterEvent(UpdateModeEnum updateModeEnum, Action action)
        {
            switch (updateModeEnum)
            {
                case UpdateModeEnum.Undefined:
                    break;
                case UpdateModeEnum.Update:
                    m_onUpdate += action.Invoke;
                    break;
                case UpdateModeEnum.FixedUpdate:
                    m_onFixedUpdate += action.Invoke;
                    break;
                case UpdateModeEnum.LateUpdate:
                    m_onLateUpdate += action.Invoke;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateModeEnum), updateModeEnum, null);
            }
        }
        
        public void UnregisterEvent(UpdateModeEnum updateModeEnum, Action action)
        {
            switch (updateModeEnum)
            {
                case UpdateModeEnum.Undefined:
                    break;
                case UpdateModeEnum.Update:
                    m_onUpdate -= action.Invoke;
                    break;
                case UpdateModeEnum.FixedUpdate:
                    m_onFixedUpdate -= action.Invoke;
                    break;
                case UpdateModeEnum.LateUpdate:
                    m_onLateUpdate -= action.Invoke;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateModeEnum), updateModeEnum, null);
            }
        }
        
        private void Update()
        {
            m_onUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            m_onFixedUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            m_onLateUpdate?.Invoke();
        }
    }

    public enum UpdateModeEnum
    {
        Undefined = 0,
        Update = 1,
        FixedUpdate = 2,
        LateUpdate = 3,
    }
}
