namespace QRCode.Engine.Toolbox.Pattern.Singleton
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    /// <summary>
    /// An unique Instance callable everywhere.
    /// </summary>
    public class MonoBehaviourSingleton<T> : SerializedMonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
#if UNITY_EDITOR
                    if (Application.isPlaying == false)
                        return null;
#endif
                    
                    var objs = FindObjectsOfType(typeof(T)) as T[];
                    if (objs.Length > 0)
                    {
                        _instance = objs[0];
                    }
                    if (objs.Length > 1)
                    {
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                    }
                    
                    if (_instance == null)
                    {
                        var obj = new GameObject();
                        obj.name =  "[SINGLETON] " + typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Check if the singleton have already an instance.
        /// </summary>
        public static bool HasInstance()
        {
            return _instance != null;
        }
    }
}