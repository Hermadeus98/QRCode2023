namespace QRCode.Framework
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = K.DatabasePath.BasePath + "Loading Screen Database", fileName = "BD_LoadingScreen")]
    public class LoadingScreenDatabase : ScriptableObjectDatabase<LoadingScreenReference>
    {
        
    }

    [Serializable]
    public struct LoadingScreenReference
    {
        [DrawWithUnity] public AssetReference LoadingScreenAssetReference;
    }
}
