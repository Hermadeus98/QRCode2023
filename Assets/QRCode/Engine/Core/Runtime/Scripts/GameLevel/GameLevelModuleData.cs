namespace QRCode.Engine.Core.GameLevels
{
    using Sirenix.OdinInspector;
    using Toolbox;
    using UnityEngine;

    public class GameLevelModuleData : ScriptableObject
    {
        [TitleGroup(Constants.InspectorGroups.Debugging)]
        [SerializeField] protected GameLevelLoadProgressionInfos m_gameLevelLoadProgressionInfos;

        public GameLevelLoadProgressionInfos GameLevelLoadProgressionInfos
        {
            get => m_gameLevelLoadProgressionInfos;
            set => m_gameLevelLoadProgressionInfos = value;
        }
    }
}