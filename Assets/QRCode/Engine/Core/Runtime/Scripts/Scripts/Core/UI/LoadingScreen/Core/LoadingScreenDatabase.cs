namespace QRCode.Engine.Core.UI.LoadingScreen
{
    using System;
    using Toolbox;
    using Sirenix.OdinInspector;
    using Toolbox.Database;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = Constants.DatabasePath.BasePath + "Loading Screen Database", fileName = "BD_LoadingScreen")]
    public class LoadingScreenDatabase : ScriptableObjectDatabase<LoadingScreenReference>
    {
        
    }

    [Serializable]
    public struct LoadingScreenReference
    {
        [DrawWithUnity] public AssetReference LoadingScreenAssetReference;
    }
}
