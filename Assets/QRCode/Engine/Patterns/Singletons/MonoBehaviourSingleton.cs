namespace QRCode.Framework.Singleton
{
    using UnityEngine;

    /// <summary>
    /// An unique Instance callable every where
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
    {
        private static T m_instance;
        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
#if UNITY_EDITOR
                    if (Application.isPlaying == false)
                        return null;
#endif
                    
                    var objs = FindObjectsOfType(typeof(T)) as T[];
                    if (objs.Length > 0)
                    {
                        m_instance = objs[0];
                    }
                    if (objs.Length > 1)
                    {
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                    }
                    
                    if (m_instance == null)
                    {
                        var obj = new GameObject();
                        obj.name =  "[SINGLETON] " + typeof(T).Name;
                        m_instance = obj.AddComponent<T>();
                    }
                }
                return m_instance;
            }
        }
    }
}