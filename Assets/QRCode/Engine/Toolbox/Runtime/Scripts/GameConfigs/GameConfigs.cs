namespace QRCode.Engine.Toolbox.GameConfigs
{
    using System;
    using UnityEngine;
    
    using System.Linq;
    
    using Sirenix.OdinInspector;
    
    using Engine.Toolbox;
    using Engine.Toolbox.Pattern.Singleton;
    using Engine.Toolbox.Helpers;


    [CreateAssetMenu(menuName = Constants.GameConfigs.GameConfigsPath)]
    public class GameConfigs : SingletonScriptableObject<GameConfigs>
    {
        [TitleGroup(Constants.InspectorGroups.References)]
        [SerializeField][ReadOnly] private GameConfigBase[] allGameConfigs;

        public T GetCatalogOfType<T>() where T : GameConfigBase
        {
            return allGameConfigs.OfType<T>().FirstOrDefault();
        }

        public GameConfigBase GetCatalogOfType(Type type)
        {
            return allGameConfigs.First(w => w.GetType() == type);
        }
        
        
        [Button]
        private void Fetch()
        {
#if UNITY_EDITOR
            var catalogs = FindAssetsHelper.FindAssetsByType<GameConfigBase>();
            allGameConfigs = catalogs as GameConfigBase[] ?? catalogs.ToArray();
#endif
        }
    }
}
