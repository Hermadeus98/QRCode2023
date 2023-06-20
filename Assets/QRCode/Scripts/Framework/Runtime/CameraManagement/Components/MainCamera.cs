namespace QRCode.Framework
{
    using Singleton;
    using UnityEngine;

    public class MainCamera : MonoBehaviourSingleton<MainCamera>
    {
        private Camera m_camera;

        public Camera Camera
        {
            get
            {
                if (m_camera == null)
                {
                    m_camera = GetComponent<Camera>();
                }

                return m_camera;
            }
        }
    }
}