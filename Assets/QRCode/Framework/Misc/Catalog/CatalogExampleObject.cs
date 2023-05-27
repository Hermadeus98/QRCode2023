namespace QRCode.Framework
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = K.MenuNameTestPath.Test + "Catalog Example")]
    public class CatalogExampleObject : CatalogObject
    {
        [TableList] [Searchable] [SerializeField]
        private CatalogExampleObjectStruct[] m_catalogExampleObjects = new CatalogExampleObjectStruct[1];
    }

    public struct CatalogExampleObjectStruct
    {
        [TabGroup("Data Group 1")] 
        public float Speed;

        [TableMatrix][TabGroup("Data Group 1")] 
        public List<Sprite> Inventory;

        [TabGroup("Data Group 2")] 
        public SceneLoadableProgressionInfos SceneLoadableProgressionInfos;
    }
}