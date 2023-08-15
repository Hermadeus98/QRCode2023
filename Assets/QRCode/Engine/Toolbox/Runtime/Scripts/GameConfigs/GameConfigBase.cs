namespace QRCode.Engine.Toolbox.GameConfigs
{
    using System.Collections.Generic;
    using System.Linq;
    using Toolbox;
    using Sirenix.OdinInspector;
    using UnityEditor;
    using UnityEngine;

    public abstract class GameConfigBase : SerializedScriptableObject
    {
        public abstract List<GameConfigDataBase> GetGameConfigData();
    }
    
    public abstract class GameConfigBase<T> : GameConfigBase where T : GameConfigDataBase
    {
        [TitleGroup(Constants.InspectorGroups.Infos)]
        [SerializeField][OnValueChanged("SetGameConfigNameInEditor")] private string m_gameConfigName = "";

        [TitleGroup("Game Configs")] [SerializeField][InfoBox("@this.GameConfigDescription")]
        private List<T> m_catalogData = new List<T>();

        protected abstract string GameConfigDescription { get; }
        public List<T> GameConfigData => m_catalogData;

        public override List<GameConfigDataBase> GetGameConfigData()
        {
            var list = new List<GameConfigDataBase>();

            for (int i = 0; i < m_catalogData.Count; i++)
            {
                list.Add(m_catalogData[i]);
            }

            return list;
        }

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
            AssetDatabase.RenameAsset(path,  Constants.GameConfigs.NamePrefix + m_gameConfigName);
#endif
        }
    }

    public class GameConfigDataBase
    {
        [TitleGroup(Constants.InspectorGroups.Settings)] 
        [SerializeField] private string m_name;

        public string Name => m_name;
    }
}
