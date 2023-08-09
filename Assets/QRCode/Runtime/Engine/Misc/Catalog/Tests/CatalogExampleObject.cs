﻿namespace QRCode.Framework
{
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = K.MenuNameTestPath.Test + "Catalog Example")]
    public class CatalogExampleObject : CatalogObject<CatalogExampleObjectStruct>
    {
        
    }

    [Serializable]
    public class CatalogExampleObjectStruct : CatalogDataBase
    {
        [TabGroup("Data Group 1")] 
        public float Speed;

        [TableMatrix][TabGroup("Data Group 1")] 
        public List<Sprite> Inventory;

        [TabGroup("Data Group 2")] 
        public GameLevelLoadProgressionInfos GameLevelLoadProgressionInfos;
    }
}