namespace QRCode.Engine.Toolbox.Pattern.Pooling
{
    using UnityEngine;

    public static class PoolSystemUtility
    {
        public static void Push(IPoolObject poolObject)
        {
            poolObject.OnPush();
            poolObject.IsAvailable = true;
            ((MonoBehaviour)poolObject).gameObject.SetActive(false);
        }

        public static void PushObject(this IPoolObject poolObject)
        {
            Push(poolObject);
        }
    }
}
