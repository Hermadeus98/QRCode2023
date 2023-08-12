namespace QRCode.Engine.Core.SceneManagement
{
    using System;
    using Toolbox;
    using Sirenix.OdinInspector;
    using Toolbox.Database;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = Constants.DatabasePath.BasePath + "Scene Database", fileName = "DB_SceneDatabase")]
    public class SceneDatabase : ScriptableObjectDatabase<SceneReference>
    {
        
    }

    [Serializable] [DrawWithUnity]
    public struct SceneReference
    {
        [SerializeField] private AssetReference m_scene;

        public AssetReference Scene => m_scene;
    }
}
