namespace QRCode.Engine.Core.GameLevels
{
    using UnityEngine;
    
    using Sirenix.OdinInspector;
    
    using Engine.Toolbox;

    public class GameLevelModuleData : ScriptableObject
    {
        [TitleGroup(Constants.InspectorGroups.Debugging)]
        [SerializeField] protected GameLevelLoadingInfo gameLevelLoadingInfo;

        public GameLevelLoadingInfo GameLevelLoadingInfo
        {
            get => gameLevelLoadingInfo;
            set => gameLevelLoadingInfo = value;
        }
    }
}