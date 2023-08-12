namespace QRCode.Engine.Toolbox.AddressableManagement
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public class ReleaseAddressableInstanceOnDestroy : MonoBehaviour
    {
        private void OnDestroy()
        {
            Addressables.Release(gameObject);
        }
    }
}