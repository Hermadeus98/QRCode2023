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
        protected override string m_databaseInformation { get => "This database must contains all loading screen necessary in the game."; }
    }

    [Serializable]
    public struct LoadingScreenReference
    {
        [DrawWithUnity] public AssetReference LoadingScreenAssetReference;
    }
}
