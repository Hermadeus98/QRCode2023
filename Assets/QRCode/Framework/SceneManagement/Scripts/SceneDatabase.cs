namespace QRCode.Framework.SceneManagement
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = K.DatabasePath.BasePath + "Scene Database", fileName = "DB_SceneDatabase")]
    public class SceneDatabase : ScriptableObjectDatabase<SceneDatabase.SceneReferenceGroup>
    {
        [Serializable][DrawWithUnity]
        public struct SceneReferenceGroup
        {
            public AssetReference[] Scenes;
        }
    }
}
