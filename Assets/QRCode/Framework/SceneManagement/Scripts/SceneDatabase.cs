namespace QRCode.Framework.SceneManagement
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = K.DatabasePath.BasePath + "Scene Database", fileName = "New SceneDatabase")]
    public class SceneDatabase : Database<SceneDatabase.SceneReferenceGroup>
    {
        [Serializable][DrawWithUnity]
        public struct SceneReferenceGroup
        {
            public AssetReference[] Scenes;
            [HideInInspector] public string SceneReferenceGroupName;
        }

        public bool TryGetSceneReferenceGroup(SceneReferenceGroupEnum sceneGroupName, out SceneReferenceGroup sceneReferenceGroup)
        {
            return TryGetDatabase(sceneGroupName.ToString(), out sceneReferenceGroup);
        }
    }
}
