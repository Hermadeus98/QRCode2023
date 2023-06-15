namespace QRCode.Framework.SceneManagement
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = K.DatabasePath.BasePath + "Level Database", fileName = "DB_LevelDatabase")]
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
