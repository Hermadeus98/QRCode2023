namespace QRCode.Framework.SceneManagement
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = K.DatabasePath.BasePath + "Scene Database", fileName = "DB_SceneDatabase")]
    public class LevelDatabase : ScriptableObjectDatabase<LevelReferenceGroup>
    {
        
    }
    
    [Serializable][DrawWithUnity]
    public struct LevelReferenceGroup
    {
        [SerializeField] private AssetReference[] m_levels;

        public AssetReference[] Levels => m_levels;
    }
}
