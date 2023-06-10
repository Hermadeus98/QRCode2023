namespace QRCode.GeneratedEnum
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using TextGenerator = Utils.TextGenerator;

    [CreateAssetMenu(menuName = "QRCode/Generated Enum", fileName = "New Generated Enum")]
    public class EnumObject : SerializedScriptableObject
    {
        [SerializeField] private string m_namespace;
        [SerializeField] private string m_enumName;
        [SerializeField] private string m_path;
        [SerializeField] private List<string> m_enum = new List<string>();
#if UNITY_EDITOR
        [ReadOnly, SerializeField] private TextAsset m_generatedFile;
#endif

#if UNITY_EDITOR
        [Button]
        private void GenerateEnumFile()
        {
            m_generatedFile = TextGenerator.GenerateCSEnum(m_path, m_enumName, m_namespace, m_enum);
        }
#endif
    }
}
