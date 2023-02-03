namespace QRCode.Gameplay.Pooling.Tests
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class PoolingTest : SerializedMonoBehaviour
    {
        [SerializeField] private PoolObjectTest m_poolObjectTest = null;
        
        private PoolList<PoolObjectTest> m_poolList = null;

        private void Start()
        {
            m_poolList = new PoolList<PoolObjectTest>(m_poolObjectTest, "PoolObjectTests");
        }

        [Button]
        private void Pool()
        {
            var poolObject = m_poolList.Get();
            poolObject.transform.position = Random.insideUnitSphere * 2f;
        }
    }
}