namespace QRCode.Engine.Core.GameLevel.Tests
{
    using UnityEngine;
    
    using Sirenix.OdinInspector;
    
    using Engine.Core.GameLevels;
    using Engine.Toolbox;

    [CreateAssetMenu]
    public class GameLevelModuleLoadableTestData : GameLevelModuleData
    {
        [TitleGroup(Constants.InspectorGroups.Settings)]
        [SerializeField] private float m_waitDuration = 1f;

        public float WaitDuration => m_waitDuration;
    }
}