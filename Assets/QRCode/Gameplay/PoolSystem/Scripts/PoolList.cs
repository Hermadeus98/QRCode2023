namespace QRCode.Gameplay.Pooling
{
    using System.Collections.Generic;
    using System.Linq;
    using Sirenix.Utilities;
    using UnityEngine;
    
    public class PoolList<T> where T : MonoBehaviour, IPoolObject
    {
        #region FIELDS
        private readonly T m_original = null;
        private readonly List<T> m_pool = new List<T>();
        private readonly string m_parentName;
        private bool m_isInitialize = false;
        private Transform m_parent = null;
        #endregion

        #region PROPERTIES
        private Transform Parent
        {
            get
            {
                if (m_parent == null)
                    m_parent = new GameObject("POOL LIST -> " + m_parentName).transform;

                return m_parent;
            }
        }
        #endregion

        #region METHODS
        public PoolList(T original, string parentName)
        {
            m_original = original;
            m_parentName = parentName;
        }

        public T Get()
        {
            if (m_pool.IsNullOrEmpty())
            {
                Initialize();
            }

            var obj = m_pool.All(w => w.IsAvailable == false) ? CreateNewObject() : m_pool.First(w => w.IsAvailable);
            obj.gameObject.SetActive(true);
            obj.OnPool();
            obj.IsAvailable = false;

            return obj;
        }
        
        public void Clear()
        {
            m_pool.ForEach(Object.Destroy);
        }

        private void Initialize()
        {
            if (!m_isInitialize)
                m_isInitialize = true;
            
            CreateNewObject();
        }

        private T CreateNewObject()
        {
            var obj = Object.Instantiate(m_original, Vector3.zero, Quaternion.identity, Parent);
            obj.gameObject.SetActive(false);
            m_pool.Add(obj);
            return obj;
        }
        #endregion
    }
}
