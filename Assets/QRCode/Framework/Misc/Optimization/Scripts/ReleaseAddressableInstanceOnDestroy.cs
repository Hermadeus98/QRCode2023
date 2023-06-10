namespace QRCode.Framework
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public class ReleaseAddressableInstanceOnDestroy : MonoBehaviour
    {
        private void OnDestroy()
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }
}
