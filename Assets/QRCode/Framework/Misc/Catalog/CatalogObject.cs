namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEditor;
    using UnityEngine;

    public abstract class CatalogObject : SerializedScriptableObject
    {
        [TitleGroup(K.InspectorGroups.Infos)]
        [SerializeField][OnValueChanged("SetCatalogNameInEditor")] private string m_catalogName = "";

        public string CatalogName => m_catalogName;

        private void SetCatalogNameInEditor()
        {
            var path = AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(path,  K.Catalog.NamePrefix + m_catalogName);
        }
    }
}
