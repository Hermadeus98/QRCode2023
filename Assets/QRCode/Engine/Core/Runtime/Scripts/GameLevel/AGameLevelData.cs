namespace QRCode.Engine.Core.GameLevels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using QRCode.Engine.Core.Tags;
    using QRCode.Engine.Debugging;
    using Sirenix.OdinInspector;

    using QRCode.Engine.Toolbox.Helpers;
    
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.Scripting;

    /// <summary>
    /// A <see cref="AGameLevelData"/> defines the tangible environment of the game, it can contains multiple scenes, contains GPE and minor logics elements.
    /// Should be a wrapper to feed in data <exception cref="AGameLevel"></exception>, must be override for each new game level.
    /// </summary>
    public abstract class AGameLevelData : ScriptableObject
    {
        [TitleGroup("References")]
        [Tooltip("References of all scenes that compose a Game Level.")]
        [SerializeField] private AssetReference[] m_gameLevelScenes = null;
        
        [Tooltip("All the Game Level Module Data used to build the Game Level Modules.")]
        [ValueDropdown("EditorGetAllGameLevelModules", IsUniqueList = true, DropdownTitle = "Select Game Level Module Object", DrawDropdownForListElements = false, ExcludeExistingValuesInList = true)]
        [SerializeField] private GameLevelModuleData[] m_gameLevelModuleData = null;
        
        public AssetReference[] GameLevelScenes => m_gameLevelScenes;

        public bool TryGetGameLevelModuleOfType<T>(out T gameLevelModuleData) where T : GameLevelModuleData
        {
#if UNITY_EDITOR
            if (CheckMultipleOccurrenceOfModuleTypes(typeof(T)))
            {
                QRLogger.DebugError<CoreTags.GameLevels>($"Multiple {nameof(GameLevelModuleData)} of type : {typeof(T)} has been found in {m_gameLevelModuleData}", this);
                
                gameLevelModuleData = null;
                return false;
            }
#endif
            
            for (int i = 0; i < m_gameLevelModuleData.Length; i++)
            {
                if (m_gameLevelModuleData[i].GetType() == typeof(T))
                {
                    gameLevelModuleData = m_gameLevelModuleData[i] as T;
                    return true;
                }
            }

            gameLevelModuleData = null;
            return false;
        }

#if UNITY_EDITOR
        private bool CheckMultipleOccurrenceOfModuleTypes(Type type)
        {
            IEnumerable<GameLevelModuleData> allOccurrences = m_gameLevelModuleData.Where(w => w.GetType() == type);
            int occurrences = allOccurrences.Count();

            return occurrences > 1;
        }
        
        /// <summary>
        /// Used by the editor.
        /// </summary>
        [Preserve]
        private IEnumerable EditorGetAllGameLevelModules()
        {
            return FindAssetsHelper.FindAssetsByType<GameLevelModuleData>();
        }
#endif
    }
}
