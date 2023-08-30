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
        protected override string m_databaseInformation { get => "This database must contains all the scenes of the games."; }
    }

    [Serializable] [DrawWithUnity]
    public struct SceneReference
    {
        [SerializeField] private string m_sceneName;
        [SerializeField] private AssetReference m_scene;

        public string SceneName => m_sceneName;
        public AssetReference Scene => m_scene;
    }
}
