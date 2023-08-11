namespace QRCode.Framework
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEditor;
    using UnityEngine;

    public abstract class GameConfigBase : SerializedScriptableObject
    {
        
    }
    
    public abstract class GameConfigBase<T> : GameConfigBase where T : GameConfigDataBase
    {
        [TitleGroup(K.InspectorGroups.Infos)]
        [SerializeField][OnValueChanged("SetGameConfigNameInEditor")] private string m_gameConfigName = "";

        [TitleGroup("Game Configs")] [SerializeField]
        private List<T> m_catalogData = new List<T>();

        public string GameConfigName => m_gameConfigName;

        public T GetDataFromId(string entry)
        {
            for (int i = 0; i < m_catalogData.Count; i++)
            {
                if (m_catalogData[i].Name == entry)
                {
                    return m_catalogData[i];
                }
            }

            return null;
        }
        
        private void SetGameConfigNameInEditor()
        {
#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(path,  K.GameConfigs.NamePrefix + m_gameConfigName);
#endif
        }
    }

    public class GameConfigDataBase
    {
        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private string m_name;

        public string Name => m_name;
    }
}
