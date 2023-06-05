namespace QRCode.Framework
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEditor;
    using UnityEngine;

    public abstract class CatalogObject : SerializedScriptableObject
    {
        
    }
    
    public abstract class CatalogObject<T> : CatalogObject where T : CatalogDataBase
    {
        [TitleGroup(K.InspectorGroups.Infos)]
        [SerializeField][OnValueChanged("SetCatalogNameInEditor")] private string m_catalogName = "";

        [TitleGroup("Catalog")] [SerializeField]
        private List<T> m_catalogData = new List<T>();

        public string CatalogName => m_catalogName;

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
        
        private void SetCatalogNameInEditor()
        {
#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(path,  K.Catalog.NamePrefix + m_catalogName);
#endif
        }
    }

    public class CatalogDataBase
    {
        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private string m_name;

        public string Name => m_name;
    }
}
