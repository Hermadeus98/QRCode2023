namespace QRCode.Framework
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = K.DatabasePath.BasePath + "Scene Database", fileName = "DB_SceneDatabase")]
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
