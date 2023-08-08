namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public class AddressableManagementTest : MonoBehaviour
    {
        [SerializeField] private AssetReference m_assetReference = null;
        
        [Button]
        private async void Instantiate()
        {
            var handle = Addressables.InstantiateAsync(m_assetReference).BindTo(gameObject);
            await handle.Task;
        }

        [Button]
        private void Destroy()
        {
            Destroy(gameObject);
        } 
    }
}
