namespace QRCode.Framework.Singleton
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    /// <summary>
    /// An unique Instance callable every where
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
    {
        [TitleGroup(K.InspectorGroups.SingletonSettingsGroup)]
        [SerializeField] private bool m_dontDestroyOnLoad = true;
        
        private static T m_instance;
        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
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
                        obj.name = "[SINGLETON] " + typeof(T).Name.ToString();
                        m_instance = obj.AddComponent<T>();
                    }
                    
                    if ((m_instance as MonoBehaviourSingleton<T>).m_dontDestroyOnLoad)
                    {
                        m_instance.transform.SetParent(null);
                        DontDestroyOnLoad(m_instance);
                    }
                    
                    (m_instance as MonoBehaviourSingleton<T>).OnInitialize();
                }
                return m_instance;
            }
        }
        
        // Optional overridable method for initializing the instance.
        protected virtual void OnInitialize()
        {
            
        }
    }
}